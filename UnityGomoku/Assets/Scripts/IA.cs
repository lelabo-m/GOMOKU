using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gomoku
{
    public enum Who { IA, PLAYER };

    public static class Exploration
    {
        public static double constante = 1.0f;
    }



    public class Node
    {
        // TMP
        public static int SuperId = 0;
        public int id;

        public double UCB;
        public int rank;
        public double reward;
        public int visit;
        public Coord cell;
        public Node parent;
        public List<Node> childs;

        public Node(Node p)
        {
            UCB = -1.0f;
            rank = (p != null) ? (p.rank + 1) : (0);
            cell = new Coord();
            visit = 0;
            reward = 0.0f;
            parent = p;
			childs = new List<Node> ();

            
            id = SuperId++;
        }
        public bool HasChild()
        {
            return ((childs.Count != 0) ? (true) : (false));
        }
        public Who  WhoPlay()
        {
            return ((rank == 0 || rank % 2 == 1) ? (Who.IA) : (Who.PLAYER));
        }
        public void Repr(ref string repr)
        {
            repr += (id) + " = [ father = " + ((this.parent != null) ? this.parent.id : 0) + " rank = " + rank + " = reward : " + reward + " visit : " + visit + " cell {" + cell.x + " " + cell.y + "} ]\n";
            string s = "";
            foreach (Node child in childs)
                child.Repr(ref s);
            repr += s;
        }
        public string Repr()
        {
            string s = (id) + " = [ father = " + ((this.parent != null) ? this.parent.id : 0) + " rank = " + rank + " = reward : " + reward + " visit : " + visit + " cell {" + cell.x + " " + cell.y + "} ]\n";
            foreach (Node child in childs)
                child.Repr(ref s);
            return s;
        }
        public void Supr()
        {
            if (parent != null)
                parent.childs.Remove(this);
            parent = null;
        }
    }



    public class MCTree
    {
        public Node root;
        public void Clear()
        {
            root = new Node(null);
        }
        public Node BestNodeUCB(Node parent)
        {
            Node    empty = new Node(parent);
            foreach (Node child in parent.childs)
                child.UCB = UCB(parent, child);
            empty.UCB = UCB(parent, empty);
            Node    result = null;
            foreach (Node child in parent.childs)
                if (result == null || result.UCB < child.UCB)
                    result = child;
            if (result == null || result.UCB < empty.UCB)
            {
                DebugConsole.Log("Create node " + empty.id);
                parent.childs.Add(empty);
                return empty;
            }
            return (result);
        }
        public Node BestNode(Node parent)
        {
            foreach (Node child in parent.childs)
                child.UCB = UCB(parent, child);
            Node result = null;
            foreach (Node child in parent.childs)
                if (result == null || result.UCB < child.UCB)
                    result = child;
            return (result);
        }
        public double UCB(Node parent, Node n) // Possible change n.reward / n.visit
        {
            return (n.reward + Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit));
        }
        public Node Selection()
        {
            Node    current = root;
            current = BestNodeUCB(current);
            while (current.HasChild())
                current = BestNodeUCB(current);
            return (current.visit != 0) ? (new Node(current)) : (current);
        }
        public Node Final()
        {
            Node current = root;
            while (current.HasChild())
                current = BestNode(current);
            return current;
        }
        public void BackProagation(Node last)
        {
            Node it = last.parent;
            while (it != null)
            {
                it.reward += last.reward;
                it.visit += 1;
                it = it.parent;
            }
        }
        public string Representation()
        {
            string repr = "Tree representation :";
            root.Repr(ref repr);
            return repr;
        }
    }




    public class MCTS_IA
    {
        public List<Map>    maps;
        public MCTree       tree;
        public int          time;

        public  MCTS_IA(int nbthread, int t)
        {
            time = t;
			this.maps = new List<Map> ();
			this.tree = new MCTree ();
            for (int i = 0; i < nbthread; ++i)
			    maps.Add(new Map(MapComponent.SIZE_MAP));
        }
        public void         OrderPawn(Node current, ref List<Node> l)
        {
            if (current.parent != tree.root)
                OrderPawn(current.parent, ref l);
            if (current.parent != tree.root)
                l.Add(current.parent);
        }
        public void         PlayGame(Node current, Map map, GameManager gm)
        {
            Coord   lastpawn = new Coord();
            Color   lastcolor = Color.Black;
            List<Node> l = new List<Node>();
            OrderPawn(current, ref l);
            // PastPlay
            foreach (Node it in l)
            {
                gm.rules.PutPawn(map, it.cell.x, it.cell.y, lastcolor);
                lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
            }
            Color winner = Color.Empty;
            int pawn = map.RandomCell(lastcolor);
            lastpawn.x = pawn / MapComponent.SIZE_MAP;
            lastpawn.y = pawn % MapComponent.SIZE_MAP;
            current.cell.x = lastpawn.x;
            current.cell.y = lastpawn.y;
            DebugConsole.Log("Random = " + pawn + " X = " + lastpawn.x + " Y = " + lastpawn.y + " Who = " + lastcolor + " " + current.Repr(), "warning");
            if (pawn == -1)
            {
                current.Supr();
                return;
            }
            current.visit = 1;
            gm.rules.PutPawn(map, lastpawn.x, lastpawn.y, lastcolor);
            winner = gm.CheckMap(lastpawn.x, lastpawn.y, map);
            lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);

            int i = 0;
            while (winner == Color.Empty && i++ < 100)
            {
                pawn = map.RandomCell(lastcolor);
                if (pawn == -1)
                    break;
                lastpawn.x = pawn / MapComponent.SIZE_MAP;
                lastpawn.y = pawn % MapComponent.SIZE_MAP;
                gm.rules.PutPawn(map, lastpawn.x, lastpawn.y, lastcolor);
                winner = gm.CheckMap(lastpawn.x, lastpawn.y, map);
                lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
            }
            current.reward = (winner == Color.Empty) ? (0.0f) : (winner == Color.Black) ? (1.0f) : (-1.0f);
            DebugConsole.Log("INFO = id = " + current.id + " Rank = " + current.rank + " Reward = " + current.reward + " VISIT = " + current.visit + " CELL = " + current.cell.x + " " + current.cell.y);
        }
        public Coord     Simulate(GameManager gm)
        {

            Stopwatch s = new Stopwatch();
            s.Start();
            // Simulation
            DebugConsole.Log("Begin Loop simulation", "warning");
            while (s.Elapsed < TimeSpan.FromMilliseconds(time))
            {
                foreach (Map m in maps)
                    m.Copy(gm.map.GetMap());
                Node tosimule = tree.Selection();
                PlayGame(tosimule, maps[0], gm);
                tree.BackProagation(tosimule);
            }
            s.Stop();

            // Final choice
            DebugConsole.Log("Exit loop simulation", "warning");
            Node final = tree.Final();
            while (final.parent != tree.root)
                final = final.parent;
            Coord result = final.cell;
            DebugConsole.Log("FINAL INFO = id = " + final.id + " Rank = " + final.rank + " Reward = " + final.reward + " VISIT = " + final.visit + " CELL = " + final.cell.x + " " + final.cell.y);
            DebugConsole.Log(tree.Representation());
            return result;
        }
        public void     Play(GameManager gm)
        {
            tree.Clear();
            DebugConsole.Log("Begin IA", "warning");
            Coord res = Simulate(gm);
            DebugConsole.Log("EXIT IA", "warning");
			gm.map.PlayOnTile (res.x, res.y);
            gm.currentPlayer().isplaying = false;
        }
    }
}
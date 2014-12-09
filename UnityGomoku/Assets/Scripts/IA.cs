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
            repr += this + " = [ " + rank + " = reward : " + reward + " visit : " + visit + " cell {" + cell.x + " " + cell.y + "} ]";
            string s = "";
            foreach (Node child in childs)
                child.Repr(ref s);
            repr += s + "\n";
        }
    }

    public class MCTree
    {
        public Node root;
        public void Clear()
        {
            root = new Node(null);
        }
        public Node BestNode(Node parent)
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
                parent.childs.Add(empty);
                return empty;
            }
            return (result);
        }
        public double UCB(Node parent, Node n) // Possible change n.reward / n.visit
        {
            return (n.reward + Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit));
        }
        public Node Selection()
        {
            Node    current = root;
            while (current.HasChild())
                current = BestNode(current);
            return BestNode(current);
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
			    maps.Add(new Map(MapComponent.SIZE_MAP, true));
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
            Color   lastcolor = Color.Empty;
            List<Node> l = new List<Node>();
            OrderPawn(current, ref l);
            foreach (Node it in l)
            {
                DebugConsole.Log("Replay pawn", "warning");
                lastcolor = (it.WhoPlay() == Who.IA) ? (Color.Black) : (Color.White);
                lastpawn = it.cell;
                gm.rules.PutPawn(map, it.cell.x, it.cell.y, lastcolor);
            }
            Color winner = gm.CheckMap(lastpawn.x, lastpawn.y, map);
            int i = 0;
            Coord lastblackpawn = lastpawn;
            while (winner == Color.Empty && i++ < 10)
            {
                lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
                int pawn = map.RandomCell(lastcolor);
				DebugConsole.Log("Random = " + pawn + " Who = " + lastcolor, "warning");
                if (pawn == -1)
                {
                    winner = Color.Empty;
                    break;
                }
                lastpawn.x = pawn / MapComponent.SIZE_MAP;
                lastpawn.y = pawn % MapComponent.SIZE_MAP;
                if (lastcolor == Color.Black)
                    lastblackpawn = lastpawn;
                gm.rules.PutPawn(map, lastpawn.x, lastpawn.y, lastcolor);
                winner = gm.CheckMap(lastpawn.x, lastpawn.y, map);
                DebugConsole.Log("Lastpawn = " + lastpawn.x + " " + lastpawn.y);
            }
            current.cell.x = lastblackpawn.x;
            current.cell.y = lastblackpawn.y;
            current.reward = (winner == Color.Empty) ? (0.0f) : (winner == Color.Black) ? (1.0f) : (-1.0f);
            current.visit = 1;
            DebugConsole.Log("INFO = Rank = " + current.rank + " Reward = " + current.reward + " VISIT = " + current.visit + " CELL = " + current.cell.x + " " + current.cell.y);
        }
        public Coord     Simulate(GameManager gm)
        {
            Coord   result = new Coord();
            Stopwatch s = new Stopwatch();
            s.Start();
            DebugConsole.Log("Begin Loop simulation", "warning");
            while (s.Elapsed < TimeSpan.FromMilliseconds(time))
            {
                DebugConsole.Log("Loop Simulation", "warning");
                Node tosimule = tree.Selection();
                PlayGame(tosimule, maps[0], gm);
                tree.BackProagation(tosimule);
            }
            s.Stop();
            DebugConsole.Log("Exit loop simulation", "warning");
            Node final = tree.Selection();
            while (final.parent != tree.root)
                final = final.parent;
            result.x = final.cell.x;
            result.y = final.cell.y;
            DebugConsole.Log(tree.Representation());
            DebugConsole.Log("FINAL INFO = Rank = " + final.rank + " Reward = " + final.reward + " VISIT = " + final.visit + " CELL = " + final.cell.x + " " + final.cell.y);
            return result;
        }
        public void     Play(GameManager gm)
        {
            foreach (Map m in maps)
                m.Copy(gm.map.GetMap());
            tree.Clear();
            DebugConsole.Log("Begin simulation", "warning");
            Coord res = Simulate(gm);
            DebugConsole.Log("End Simulation", "warning");
			gm.map.PlayOnTile (res.x, res.y);
        }
    }
}
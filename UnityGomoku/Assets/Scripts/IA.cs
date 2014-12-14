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
        public static double constante = 1.5f;
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
        public bool final;

        public Node(Node p)
        {
            UCB = -1.0f;
            rank = (p != null) ? (p.rank + 1) : (0);
            cell = new Coord();
            visit = 0;
            reward = 0.0f;
            parent = p;
			childs = new List<Node> ();
            final = false;

            
            id = SuperId++;
        }

        public void Shuffle()
        {
            Randomizer rnd = new Randomizer();
            int n = childs.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Rand().Next(n + 1);
                Node value = childs[k];
                childs[k] = childs[n];
                childs[n] = value;
            }
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
        public int Size()
        {
            int i = 0;
            foreach (Node child in childs)
                i += child.Size();
            return i + 1;
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
            parent.Shuffle();
            Node    empty = new Node(parent);
            foreach (Node child in parent.childs)
                child.UCB = UCB(parent, child);
            empty.UCB = UCB(parent, empty);
            Node    result = null;
            foreach (Node child in parent.childs)
            {
                if (result == null || result.UCB < child.UCB)
                    result = child;
                child.UCB = -1.0f;
            }
            if (result == null || result.UCB < empty.UCB)
                return empty;
            return (result);
        }
        public Node BestNode(Node parent)
        {
            parent.Shuffle();
            foreach (Node child in parent.childs)
                child.UCB = UCB(parent, child);
            Node result = null;
            foreach (Node child in parent.childs)
            {
                if (result == null || result.UCB < child.UCB)
                    result = child;
                child.UCB = -1.0f;
            }
            return (result);
        }
        public double UCB(Node parent, Node n) // Possible change n.reward / n.visit
        {
            return (Math.Abs(n.reward / n.visit) + (Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit)));
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
        public bool BackProagation(Node last)
        {
            Node it = last;
            if (last.final == true)
                last.reward += 1.0f;
            if (last.parent != null)
                last.parent.childs.Add(last);
            it = last.parent;
            while (it != null)
            {
                it.reward += last.reward;
                it.visit += 1;
                it = it.parent;
            }
            return false;
        }
        public string Representation()
        {
            string repr = "Tree representation :";
            root.Repr(ref repr);
            return repr;
        }
        public int Size()
        {
            return root.Size();
        }
    }


    public class MCTS_IA
    {
        public List<Map>    maps;
        public MCTree       tree;
        public int          time;
        public int nbthreads;

        public  MCTS_IA(int nb, int t)
        {
            time = t;
            nbthreads = nb;
			this.maps = new List<Map> ();
			this.tree = new MCTree ();
            for (int i = 0; i < nbthreads; ++i)
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
            Color   lastcolor = Color.Black;
            List<Node> l = new List<Node>();
            OrderPawn(current, ref l);
            foreach (Node it in l)
            {
                gm.rules.PutPawn(map, it.cell.x, it.cell.y, lastcolor);
                lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
            }
            Color winner = Color.Empty;
            Coord pawn = map.RandomCell(lastcolor);
			if (pawn == null)
            {
			    current.Supr();
				return;
			}
			current.cell.x = pawn.x;
			current.cell.y = pawn.y;
            current.visit = 1;
            gm.rules.PutPawn(map, pawn.x, pawn.y, lastcolor);
            winner = gm.CheckMap(pawn.x, pawn.y, map);
            lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
            if (winner == Color.Black)
                current.final = true;
            int i = 0;
            while (winner == Color.Empty && ++i < 365)
            {
                pawn = map.RandomCell(lastcolor);
				if (pawn == null)
					break;
				gm.rules.PutPawn(map, pawn.x, pawn.y, lastcolor);
				winner = gm.CheckMap(pawn.x, pawn.y, map);
				lastcolor = (lastcolor == Color.Black) ? (Color.White) : (Color.Black);
            }
            current.reward = (winner == Color.Empty) ? (0.0f) : (winner == Color.Black) ? (1.0f) : (-1.0f);
        }
        public Coord     Simulate(GameManager gm)
        {

            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromMilliseconds(time))
            {
                List<Simulation>    threads = new List<Simulation>();
                List<Node>          tosimule = new List<Node>();
                Counter             val = new Counter();
                foreach (Map m in maps)
                    m.Copy(gm.map.GetMap());
                for (int i = 0; i < nbthreads - 1; ++i)
                {
                    Node todo = tree.Selection();
                    tosimule.Add(todo);
                    threads.Add(new Simulation(this, todo, maps[i], gm, val));
                    threads[i].Start();
                }
                Node current = tree.Selection();
                tosimule.Add(current);
                PlayGame(current, maps[nbthreads - 1], gm);
                val.Inc();
                while (val.Get() != nbthreads)
                {
                    foreach (Simulation thread in threads)
                        thread.Update();
                }
                foreach (Simulation thread in threads)
                    thread.Abort();
                threads.Clear();
                foreach (Node todo in tosimule)
                    tree.BackProagation(todo);
            }
            s.Stop();

            // Final choice
            //DebugConsole.Log("Exit loop! Tree size = " + tree.Size(), "warning");
            //DebugConsole.Log(tree.Representation());
            Node final = tree.Final();
            while (final.parent != tree.root)
                final = final.parent;
            Coord result = final.cell;
            return result;
        }
        public void     Play(GameManager gm)
        {
            tree.Clear();
            Coord res = Simulate(gm);
			gm.map.PlayOnTile (res.x, res.y);
            gm.currentPlayer().isplaying = false;
        }
    }
}
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

        public Node(Node p, Coord c)
        {
            UCB = -1.0f;
            rank = (p != null) ? (p.rank + 1) : (0);
            cell = c;
            visit = 0;
            reward = 0.0f;
            parent = p;
        }
        public bool HasChild()
        {
            return ((childs.Count != 0) ? (true) : (false));
        }
        public Who  WhoPlay()
        {
            return ((rank == 0 || rank % 2 == 1) ? (Who.IA) : (Who.PLAYER));
        }
    }

    public class MCTree
    {
        public Node root;
        public void Clear()
        {
            root = new Node(null, null);
        }
        public Node BestNode(Node parent)
        {
            Node    empty = new Node(parent, null);
            foreach (Node child in parent.childs)
                child.UCB = UCB(parent, child);
            empty.UCB = UCB(parent, empty);
            Node    result = null;
            foreach (Node child in parent.childs)
                if (result == null || result.UCB < child.UCB)
                    result = child;                            
            return ((result == null || result.UCB < empty.UCB) ? (empty) : (result));
        }
        public double UCB(Node parent, Node n) // Possible change n.reward / n.visit
        {
            return (n.reward + Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit));
        }
        public Node Selection()
        {
            Node    current = root;
            while (current.HasChild() || current == root)
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
            }
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
            for (int i = 0; i < nbthread; ++i)
			    maps.Add(new Map(MapComponent.SIZE_MAP, true));
        }
        public void         OrderPawn(Node current, ref List<Node> l)
        {
            if (current.parent != tree.root)
                OrderPawn(current.parent, ref l);
            l.Add(current.parent);
        }
        public void         PlayGame(Node current, Map map, GameManager gm)
        {
            List<Node> l = new List<Node>();
            OrderPawn(current, ref l);
            foreach (Node it in l)
            {
                Color   c = (it.WhoPlay() == Who.IA) ? (Color.Black) : (Color.White);
                map.PutPawn(it.cell.x, it.cell.y, c);
                gm.rules.UpdateMap(map, it.cell.x, it.cell.y);
            }
            // while game not finish
            // play pawn
            // state of game
            // -------------
            // fill the node

            /*print ("Random = " + map.GetMap().RandomCell(Gomoku.Color.White));
            print ("Random = " + map.GetMap().RandomCell(Gomoku.Color.Black));*/
        }
        public Coord     Simulate(GameManager gm)
        {
            Coord   result = new Coord();
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromMilliseconds(time))
            {
                Node tosimule = tree.Selection();
                PlayGame(tosimule, maps[0], gm);
            }
            s.Stop();
            Node final = tree.Selection();
            while (final.parent != tree.root)
                final = final.parent;
            result.x = final.cell.x;
            result.y = final.cell.y;
            return result;
        }
        public void     Play(GameManager gm)
        {
            foreach (Map m in maps)
                m.Copy(gm.map.GetMap());
            tree.Clear();
            Coord res = Simulate(gm);
            gm.map.PutPawn(res.x, res.y, Color.Black);
        }
    }
}
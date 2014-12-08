﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
        public double UCB(Node parent, Node n)
        {
            return (n.reward + Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit));
        }
        public Node Extend()
        {
            Node    current = root;
            while (current.HasChild() || current == root)
                current = BestNode(current);
            return current;
        }
    }

    public class MCTS_IA
    {
        public List<Map>    maps;
        public MCTree       tree;

        public  MCTS_IA(int nbthread)
        {
            for (int i = 0; i < nbthread; ++i)
			    maps.Add(new Map(MapComponent.SIZE_MAP, true));
        }
        public void         PlayGame(Node current, Map map)
        {
            //Node it = current;
            //List<Node>  l;
            //while (it != tree.root)
            //{
                
            //}

            //if (!rules.PutPawn(map, x, y) || (rules.DoubleThree && rules.IsDoubleThree(map, x, y, color)))
            //    return false;
            //map.PutPawn(x, y, color);
            //rules.UpdateMap(map, x, y);
        }
        public Coord     Simulate(GameManager gm)
        {
            Coord   result = new Coord();
            // while time
            //    Find Node
            //       PlayGame(Node, Map);
            //   time -= elapsed;
            // result = Find Best Result
            return result;
        }
        public void     Play(GameManager gm)
        {
            foreach (Map m in maps)
                m.Copy(gm.map.GetMap());
            tree.Clear();
            Coord res = Simulate(gm);
            gm.map.PutPawn(res.x, res.y, gm.currentPlayer().color);
        }
    }
}
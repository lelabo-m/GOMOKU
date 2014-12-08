using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gomoku
{
    public static class Exploration
    {
        public static double constante = 1.5f;
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

        public Node(Node p, int r, Coord c)
        {
            UCB = -1.0f;
            rank = r;
            cell = c;
            visit = 0;
            reward = 0.0f;
            parent = p;
        }
    }

    public class Tree
    {
        public Node root;
        public void Clear()
        {
            root = new Node(null, 0, null);
        }
        public Node BestNode(Node parent)
        {
            
            return parent;
        }
        public double UCB(Node parent, Node n)
        {
            return (n.reward + Exploration.constante * Math.Sqrt(Math.Log(parent.visit) / n.visit));
        }
        public Node Extend()
        {
            return root;
        }
    }
}

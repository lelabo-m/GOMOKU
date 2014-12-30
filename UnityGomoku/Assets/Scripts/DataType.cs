using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gomoku
{
    public enum Orientation { NORTH, NORTHWEST, WEST, SOUTHWEST, SOUTH, SOUTHEAST, EAST, NORTHEAST };

    public class Coord
    {
        public int x;
        public int y;
    }

    public class Vector
    {
        public Coord begin;
        public Coord end;
        public Orientation ori;

        public Vector()
        {
            begin = new Coord();
            end = new Coord();
        }

        public Vector(Coord b, Coord e)
        {
            begin = b;
            end = e;
        }

        public Vector(int u, int v, int i, int j)
        {
            begin = new Coord();
            end = new Coord();
            begin.x = u;
            begin.y = v;
            end.x = i;
            end.y = j;
        }

        // u->v * i->j
        public bool IntersectSegment(Vector other, ref Coord inter)
        {
			float dir_a, dir_p, ori_b, ori_d;

			dir_a = (end.y - begin.y) / (end.x - begin.x);
			ori_b = end.y - dir_a * end.x;
			dir_p = (other.end.y - other.begin.y) / (other.end.x - other.begin.x);
			ori_d = other.end.y - dir_p * other.end.x;

			inter.x = (int)((ori_d - ori_b) / (dir_a - dir_p));
			inter.y = (int)(dir_a * inter.x + ori_b);





			float s_numer, t_numer, denom, t;
            // Boundingbox
            Coord BoxUV = new Coord();
            Coord BoxIJ = new Coord();
            BoxUV.x = end.x - begin.x;
            BoxUV.y = end.y - begin.y;
            BoxIJ.x = other.end.x - other.begin.x;
            BoxIJ.y = other.end.y - other.begin.y;

            // Check Collinear
            denom = BoxUV.x * BoxIJ.y - BoxIJ.x * BoxUV.y;
            if (denom == 0)
                return false;
            bool denomPositive = denom > 0;

            // Check Collision
            Coord distance = new Coord();
            distance.x = begin.x - other.begin.x;
            distance.y = begin.y - other.begin.y;
            s_numer = BoxUV.x * distance.y - BoxUV.y * distance.x;
            if ((s_numer < 0) == denomPositive)
                return false;
            t_numer = BoxIJ.x * distance.y - BoxIJ.y * distance.x;
            if ((t_numer < 0) == denomPositive)
                return false;
            if (((s_numer > denom) == denomPositive) || ((t_numer > denom) == denomPositive))
                return false;

            // Collision detected
            t = t_numer / denom;
            float x, y;
            x = begin.x + (t * BoxUV.x);
            y = begin.y + (t * BoxUV.y);
            // Specific case for a boardgame
            if (x != (int)x || y != (int)y)
                return false;
            inter.x = (int)x;
            inter.y = (int)y;
            return true;
        }
    }

    public class DoubleThree
    {
        public Vector       three;
        public Vector       two;
        public List<Coord>  cellblocked;

        public DoubleThree(Vector th, Vector tw)
        {
            three = th;
            two = tw;
        }
    }
}
using UnityEngine;
using System.Collections;

enum Orientation { NORTH, NORTHWEST, WEST, SOUTHWEST, SOUTH, SOUTHEAST, EAST, NORTHEAST }

public class Coord
{
    public int x;
    public int y;
}

public class Vector
{
    public Coord        begin;
    public Coord        end;
    public Orientation ori;
}

public class ThreeFree : Vector
{
}

public class Segment : Vector
{
}

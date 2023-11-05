using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tile class with variables needed for a* algorithm
public class Tile
{
    public int x { get; }
    public int y { get; }
    public bool isWalkable { get; set; }
    public double g { get; set; }
    public double h { get; set; }
    public double f => g + h;
    public Tile parent { get; set; }

    //constructor
    public Tile(int _x, int _y, bool _isWalkable)
    {
        x = _x;
        y = _y;
        isWalkable = _isWalkable;
        g = 0;
        h = 0;
        parent = null;
    }
}

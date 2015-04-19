using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GridPosition
{
    public int X { get; set; }
    public int Y { get; set; }

    public GridPosition(int x, int y)
    {
        // TODO: Complete member initialization
        this.X = x;
        this.Y = y;
    }
    public static GridPosition operator +(GridPosition c1, GridPosition c2)
    {
        return new GridPosition(c1.X + c2.X, c1.Y + c2.Y);
    }
}


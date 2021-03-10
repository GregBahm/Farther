using System;
using System.Collections.Generic;
using System.Linq;


public class DesignationsGrid
{
    public DesignationGridCell[,] Cells { get; }

    public int Width { get; }
    public int Height { get; }

    public DesignationsGrid(int width, 
        int height)
    {
        Width = width;
        Height = height;
        Cells = InitializeGrid();
    }

    private DesignationGridCell[,] InitializeGrid()
    {
        DesignationGridCell[,] ret = new DesignationGridCell[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ret[x, y] = new DesignationGridCell(x, y, this);
            }
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ret[x, y].InitializeNeighbors(ret);
            }
        }
        return ret;
    }
}

public class Designation
{
}

public class DesignationGridCell
{
    private readonly DesignationsGrid grid;
    public IEnumerable<DesignationGridCell> Neighbors { get; private set; }

    public int X { get; }
    public int Y { get; }

    public Designation FilledWith { get; }

    public DesignationGridCell(int x, int y, DesignationsGrid grid)
    {
        this.grid = grid;
        X = x;
        Y = y;
    }

    public void InitializeNeighbors(DesignationGridCell[,] gridCells)
    {
        Neighbors = GetNeighbors(gridCells).ToArray();
    }

    private IEnumerable<DesignationGridCell> GetNeighbors(DesignationGridCell[,] gridCells)
    {
        int x = X - 1;
        int y = Y - 1;
        if (x >= 0 && y >= 0)
            yield return gridCells[x, y];
        if (x < grid.Width - 1 && y >= 0)
            yield return gridCells[x + 1, y];
        if (x >= 0 && y < grid.Height - 1)
            yield return gridCells[x, y + 1];
        if (x < grid.Width - 1 && y < grid.Height - 1)
            yield return gridCells[x + 1, y + 1];
    }
}
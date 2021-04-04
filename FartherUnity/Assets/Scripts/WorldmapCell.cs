using System.Collections.Generic;
using System.Linq;

public class WorldmapCell
{
    private readonly Worldmap grid;
    public IEnumerable<WorldmapCell> Neighbors { get; private set; }

    public int X { get; }
    public int Y { get; }

    public WorldmapState State { get; set;  }

    public WorldmapCell(int x, int y, Worldmap grid)
    {
        this.grid = grid;
        X = x;
        Y = y;
        State = new WorldmapState();
    }

    public void InitializeNeighbors(WorldmapCell[,] gridCells)
    {
        Neighbors = GetNeighbors(gridCells).ToArray();
    }

    private IEnumerable<WorldmapCell> GetNeighbors(WorldmapCell[,] gridCells)
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
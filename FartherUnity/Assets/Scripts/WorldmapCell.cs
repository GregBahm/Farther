using System;
using System.Dynamic;
using System.Linq;
using UnityEngine.EventSystems;

public class WorldmapCell
{
    public Neighbors<WorldmapCell> Neighbors { get; private set; }

    public int X { get; }
    public int Y { get; }

    private WorldmapState state;
    public WorldmapState State 
    { 
        get => state;
        set 
        {
            if(state != value)
            {
                state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler StateChanged;

    public WorldmapCell(int x, int y)
    {
        X = x;
        Y = y;
        State = new WorldmapState();
    }

    public void InitializeNeighbors(WorldmapCell[,] gridCells)
    {
        Neighbors = GetNeighbors(gridCells);
    }

    private Neighbors<WorldmapCell> GetNeighbors(WorldmapCell[,] map)
    {
        WorldmapCell upRight   = TryGetTile(map, X    , Y-1);
        WorldmapCell right     = TryGetTile(map, X -1 , Y);
        WorldmapCell downRight = TryGetTile(map, X -1 , Y + 1);
        WorldmapCell downLeft  = TryGetTile(map, X    , Y + 1);
        WorldmapCell left      = TryGetTile(map, X + 1, Y);
        WorldmapCell upLeft    = TryGetTile(map, X + 1, Y -1);
        return new Neighbors<WorldmapCell>(
            upRight,
            right,
            downRight,
            downLeft,
            left,
            upLeft
            );
    }

    private WorldmapCell TryGetTile(WorldmapCell[,] map, int x, int y)
    {
        int maxX = map.GetLength(0) - 1;
        int maxY = map.GetLength(1) - 1;
        if(x < 0 || y < 0 || x > maxX || y > maxY)
        {
            return null;
        }
        return map[x, y];
    }

    internal WorldmapStateWithNeighbors GetStateWithNeighbors()
    {
        Neighbors<WorldmapState> neighbors = new Neighbors<WorldmapState>(
            Neighbors.UpRight?.State,
            Neighbors.Right?.State,
            Neighbors.DownRight?.State,
            Neighbors.DownLeft?.State,
            Neighbors.Left?.State,
            Neighbors.UpLeft?.State);
        return new WorldmapStateWithNeighbors(State,
            neighbors);
    }
}

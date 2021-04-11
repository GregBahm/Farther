using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using UnityEngine.EventSystems;

public class WorldmapCell
{
    private readonly Worldmap worldmap;
    public Neighbors<string> NeighborsLookup { get; }
    public IEnumerable<WorldmapCell> Neighbors
    {
        get
        {
            return NeighborsLookup.Select(item => worldmap.TryGetCellAt(item))
                .Where(item => item != null);
        }
    }

    public int X { get; }
    public int Y { get; }
    public string MapKey { get; }

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

    public WorldmapCell(int x, int y, Worldmap worldmap)
    {
        X = x;
        Y = y;
        State = new WorldmapState();
        this.worldmap = worldmap;
        MapKey = GetCellKey(x, y);
        NeighborsLookup = GetNeighborsLookup();
    }

    private Neighbors<string> GetNeighborsLookup()
    {
        return new Neighbors<string>(
            GetCellKey(NeighborOffset.Offsets[0]),
            GetCellKey(NeighborOffset.Offsets[1]),
            GetCellKey(NeighborOffset.Offsets[2]),
            GetCellKey(NeighborOffset.Offsets[3]),
            GetCellKey(NeighborOffset.Offsets[4]),
            GetCellKey(NeighborOffset.Offsets[5])
        );
    }

    internal WorldmapStateWithNeighbors GetStateWithNeighbors()
    {
        Neighbors<WorldmapState> neighbors = new Neighbors<WorldmapState>(
            worldmap.TryGetCellAt(NeighborsLookup.UpRight)?.State,
            worldmap.TryGetCellAt(NeighborsLookup.Right)?.State,
            worldmap.TryGetCellAt(NeighborsLookup.DownRight)?.State,
            worldmap.TryGetCellAt(NeighborsLookup.DownLeft)?.State,
            worldmap.TryGetCellAt(NeighborsLookup.Left)?.State,
            worldmap.TryGetCellAt(NeighborsLookup.UpLeft)?.State);
        return new WorldmapStateWithNeighbors(State,
            neighbors);
    }

    private string GetCellKey(NeighborOffset offset)
    {
        return GetCellKey(offset.X + X, offset.Y + Y);
    }

    public static string GetCellKey(int x, int y)
    {
        return x + " " + y;
    }
}

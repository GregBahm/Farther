using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using UnityEngine.EventSystems;

public class WorldmapSlot
{
    private readonly Worldmap worldmap;
    public Neighbors<string> NeighborsLookup { get; }
    public IEnumerable<WorldmapSlot> Neighbors
    {
        get
        {
            return NeighborsLookup.Select(item => worldmap.TryGetSlotAt(item))
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
                if(state != null)
                {
                    state.OnRemovedFromMap();
                }
                state = value;
                if(state != null)
                {
                    state.OnAddedToMap(this);
                }
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler StateChanged;

    public WorldmapSlot(int x, int y, Worldmap worldmap)
    {
        X = x;
        Y = y;
        State = new SitelessTile(new TerrainState());
        this.worldmap = worldmap;
        MapKey = GetSlotKey(x, y);
        NeighborsLookup = GetNeighborsLookup();
    }

    private Neighbors<string> GetNeighborsLookup()
    {
        return new Neighbors<string>(
            GetSlotKey(NeighborOffset.Offsets[0]),
            GetSlotKey(NeighborOffset.Offsets[1]),
            GetSlotKey(NeighborOffset.Offsets[2]),
            GetSlotKey(NeighborOffset.Offsets[3]),
            GetSlotKey(NeighborOffset.Offsets[4]),
            GetSlotKey(NeighborOffset.Offsets[5])
        );
    }

    internal WorldmapStateWithNeighbors GetStateWithNeighbors()
    {
        Neighbors<WorldmapState> neighbors = new Neighbors<WorldmapState>(
            worldmap.TryGetSlotAt(NeighborsLookup.UpRight)?.State,
            worldmap.TryGetSlotAt(NeighborsLookup.Right)?.State,
            worldmap.TryGetSlotAt(NeighborsLookup.DownRight)?.State,
            worldmap.TryGetSlotAt(NeighborsLookup.DownLeft)?.State,
            worldmap.TryGetSlotAt(NeighborsLookup.Left)?.State,
            worldmap.TryGetSlotAt(NeighborsLookup.UpLeft)?.State);
        return new WorldmapStateWithNeighbors(State,
            neighbors);
    }

    private string GetSlotKey(NeighborOffset offset)
    {
        return GetSlotKey(offset.X + X, offset.Y + Y);
    }

    public static string GetSlotKey(int x, int y)
    {
        return x + " " + y;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using UnityEngine.EventSystems;

public class WorldmapPosition
{
    private readonly Worldmap worldmap;
    public Neighbors<string> NeighborsLookup { get; }
    public IEnumerable<WorldmapPosition> Neighbors
    {
        get
        {
            return NeighborsLookup.Select(item => worldmap.TryGetPositionAt(item))
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

    public WorldmapPosition(int x, int y, Worldmap worldmap)
    {
        X = x;
        Y = y;
        state = new SitelessTile(new TerrainState());
        this.worldmap = worldmap;
        MapKey = GetPositionKey(x, y);
        NeighborsLookup = GetNeighborsLookup();
    }

    private Neighbors<string> GetNeighborsLookup()
    {
        return new Neighbors<string>(
            GetPositionKey(NeighborOffset.Offsets[0]),
            GetPositionKey(NeighborOffset.Offsets[1]),
            GetPositionKey(NeighborOffset.Offsets[2]),
            GetPositionKey(NeighborOffset.Offsets[3]),
            GetPositionKey(NeighborOffset.Offsets[4]),
            GetPositionKey(NeighborOffset.Offsets[5])
        );
    }

    private string GetPositionKey(NeighborOffset offset)
    {
        return GetPositionKey(offset.X + X, offset.Y + Y);
    }

    public static string GetPositionKey(int x, int y)
    {
        return x + " " + y;
    }
}

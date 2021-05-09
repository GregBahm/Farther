using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using UnityEngine.EventSystems;

public class MapCellPosition
{
    public Map Map { get; }
    public Neighbors<string> NeighborsLookup { get; }
    public IEnumerable<MapCellPosition> Neighbors
    {
        get
        {
            return NeighborsLookup.Select(item => Map.TryGetPositionAt(item))
                .Where(item => item != null);
        }
    }

    public int X { get; }
    public int Y { get; }
    public string MapKey { get; }

    private MapCellState state;
    public MapCellState State 
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
                    state.OnAddedToMap();
                }
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler StateChanged;

    public MapCellPosition(int x, int y, Map map)
    {
        X = x;
        Y = y;
        state = new SitelessState(this, new TerrainState());
        Map = map;
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

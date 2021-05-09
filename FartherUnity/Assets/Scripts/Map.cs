using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Map : IEnumerable<MapCell>
{
    public Game Game { get; }

    public event EventHandler<MapCell> CellAdded;

    private readonly Dictionary<string, MapCell> cells = new Dictionary<string, MapCell>();

    public Map(Game gameState)
    {
        Game = gameState;
    }

    public MapCell AddCell(int x, int y)
    {
        MapCell ret = new MapCell(x, y, this);
        cells.Add(ret.MapKey, ret);
        CellAdded?.Invoke(this, ret);
        return ret;
    }

    public MapCell TryGetCellAt(string cellKey)
    {
        if (cells.ContainsKey(cellKey))
        {
            return cells[cellKey];
        }
        return null;
    }

    public MapCell TryGetCellAt(int x, int y)
    {
        string cellKey = MapCell.GetPositionKey(x, y);
        return TryGetCellAt(cellKey);
    }


    public void EnsureCellAndNeighborsExist(int x, int y)
    {
        CreateIfNotExistant(x, y);
        foreach (NeighborOffset offset in NeighborOffset.Offsets)
        {
            CreateIfNotExistant(x + offset.X, y + offset.Y);
        }
    }

    private void CreateIfNotExistant(int x, int y)
    {
        if (TryGetCellAt(x, y) == null)
        {
            AddCell(x, y);
        }
    }

    public IEnumerator<MapCell> GetEnumerator()
    {
        return cells.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

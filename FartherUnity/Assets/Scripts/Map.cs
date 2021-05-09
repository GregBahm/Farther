using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Map : IEnumerable<MapCell>
{
    public Game Game { get; }

    private readonly Dictionary<string, MapCell> cells = new Dictionary<string, MapCell>();

    public Map(Game gameState)
    {
        Game = gameState;
    }

    public MapCell AddCell(int x, int y)
    {
        MapCell ret = new MapCell(x, y, this);
        cells.Add(ret.MapKey, ret);
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

    public IEnumerator<MapCell> GetEnumerator()
    {
        return cells.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

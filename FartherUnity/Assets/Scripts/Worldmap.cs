using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Worldmap : IEnumerable<WorldmapCell>
{
    public static Vector2 AscendingTileOffset { get; } = new Vector2(1, -1.73f).normalized;
    private readonly Dictionary<string, WorldmapCell> cells = new Dictionary<string, WorldmapCell>();

    public WorldmapCell AddCell(int x, int y)
    {
        WorldmapCell newCell = new WorldmapCell(x, y, this);
        cells.Add(newCell.MapKey, newCell);
        return newCell;
    }

    public WorldmapCell TryGetCellAt(string cellKey)
    {
        if (cells.ContainsKey(cellKey))
        {
            return cells[cellKey];
        }
        return null;
    }

    public WorldmapCell TryGetCellAt(int x, int y)
    {
        string cellKey = WorldmapCell.GetCellKey(x, y);
        return TryGetCellAt(cellKey);
    }

    public IEnumerator<WorldmapCell> GetEnumerator()
    {
        return cells.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

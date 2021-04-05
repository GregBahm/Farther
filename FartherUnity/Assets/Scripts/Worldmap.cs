using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Worldmap : IEnumerable<WorldmapCell>
{
    public static Vector2 AscendingTileOffset { get; } = new Vector2(1, -1.73f).normalized;
    private readonly HashSet<WorldmapCell> hash;
    private readonly WorldmapCell[,] cells;

    public int Width { get; }
    public int Height { get; }

    public WorldmapCell this[int x, int y]
    {
        get
        {
            return cells[x, y];
        }
    }

    public Worldmap(int width,
        int height)
    {
        Width = width;
        Height = height;
        cells = Initialize();
        hash = CreateHash();
    }

    private HashSet<WorldmapCell> CreateHash()
    {
        HashSet<WorldmapCell> ret = new HashSet<WorldmapCell>();
        foreach (var item in cells)
        {
            ret.Add(item);
        }
        return ret;
    }

    private WorldmapCell[,] Initialize()
    {
        WorldmapCell[,] ret = new WorldmapCell[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ret[x, y] = new WorldmapCell(x, y);
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

    public IEnumerator<WorldmapCell> GetEnumerator()
    {
        return hash.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

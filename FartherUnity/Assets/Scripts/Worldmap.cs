using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Worldmap : IEnumerable<WorldmapSlot>
{
    public static Vector2 AscendingTileOffset { get; } = new Vector2(1, -1.73f).normalized;
    private readonly Dictionary<string, WorldmapSlot> slots = new Dictionary<string, WorldmapSlot>();

    public WorldmapSlot AddSlot(int x, int y)
    {
        WorldmapSlot newSlot = new WorldmapSlot(x, y, this);
        slots.Add(newSlot.MapKey, newSlot);
        return newSlot;
    }

    public WorldmapSlot TryGetSlotAt(string slotKey)
    {
        if (slots.ContainsKey(slotKey))
        {
            return slots[slotKey];
        }
        return null;
    }

    public WorldmapSlot TryGetSlotAt(int x, int y)
    {
        string cellKey = WorldmapSlot.GetSlotKey(x, y);
        return TryGetSlotAt(cellKey);
    }

    public IEnumerator<WorldmapSlot> GetEnumerator()
    {
        return slots.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

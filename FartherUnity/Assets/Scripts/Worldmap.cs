using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Worldmap : IEnumerable<WorldmapPosition>
{
    public GameState GameState { get; }

    private readonly Dictionary<string, WorldmapPosition> slots = new Dictionary<string, WorldmapPosition>();

    public Worldmap(GameState gameState)
    {
        GameState = gameState;
    }

    public WorldmapPosition AddSlot(int x, int y)
    {
        WorldmapPosition newSlot = new WorldmapPosition(x, y, this);
        slots.Add(newSlot.MapKey, newSlot);
        return newSlot;
    }

    public WorldmapPosition TryGetPositionAt(string positionKey)
    {
        if (slots.ContainsKey(positionKey))
        {
            return slots[positionKey];
        }
        return null;
    }

    public WorldmapPosition TryGetPositionAt(int x, int y)
    {
        string cellKey = WorldmapPosition.GetPositionKey(x, y);
        return TryGetPositionAt(cellKey);
    }

    public IEnumerator<WorldmapPosition> GetEnumerator()
    {
        return slots.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

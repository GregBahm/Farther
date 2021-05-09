using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class Map : IEnumerable<MapCellPosition>
{
    public Game Game { get; }

    private readonly Dictionary<string, MapCellPosition> slots = new Dictionary<string, MapCellPosition>();

    public Map(Game gameState)
    {
        Game = gameState;
    }

    public MapCellPosition AddPosition(int x, int y)
    {
        MapCellPosition newPosition = new MapCellPosition(x, y, this);
        slots.Add(newPosition.MapKey, newPosition);
        return newPosition;
    }

    public MapCellPosition TryGetPositionAt(string positionKey)
    {
        if (slots.ContainsKey(positionKey))
        {
            return slots[positionKey];
        }
        return null;
    }

    public MapCellPosition TryGetPositionAt(int x, int y)
    {
        string cellKey = MapCellPosition.GetPositionKey(x, y);
        return TryGetPositionAt(cellKey);
    }

    public IEnumerator<MapCellPosition> GetEnumerator()
    {
        return slots.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

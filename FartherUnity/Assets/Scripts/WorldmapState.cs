using System;
using System.Diagnostics;
using System.Dynamic;

public class WorldmapState
{
    public MapTerrainType Terrain { get; }
    public int Temperature { get; }
    public bool Hill { get; }
    public bool River { get; }

    public WorldmapState(
        MapTerrainType terrain = default,
        int temperature = 0,
        bool hill = false, 
        bool river = false)
    {
        Terrain = terrain;
        Temperature = temperature;
        Hill = hill;
        River = river;
    }
    
    public WorldmapStateBuilder ToBuilder()
    {
        return new WorldmapStateBuilder(this);
    }
}


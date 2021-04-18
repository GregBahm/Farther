using System;
using System.Diagnostics;
using System.Dynamic;

public class WorldmapState
{
    public MapTerrainType Terrain { get; }
    public int Temperature { get; }
    public bool Hill { get; }
    public bool River { get; }

    public SiteType SiteType { get; }

    public WorldmapState(
        MapTerrainType terrain = default,
        int temperature = 0,
        bool hill = false, 
        bool river = false,
        SiteType siteType = SiteType.None)
    {
        Terrain = terrain;
        Temperature = temperature;
        Hill = hill;
        River = river;
        SiteType = siteType;
    }
    
    public WorldmapStateBuilder ToBuilder()
    {
        return new WorldmapStateBuilder(this);
    }
}

public abstract class SiteState
{
    // Update method
}

public enum SiteType
{
    None = 0,
    DragonLair,
    ForestVillage,
    MagicalBathouse,
    City
}

public class WorldmapState<TSiteState> where TSiteState : SiteState
{
    public SiteType Type { get; }
    public TSiteState SiteState { get; }
}
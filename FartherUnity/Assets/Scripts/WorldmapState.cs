using System;
using System.Diagnostics;
using System.Dynamic;

public class WorldmapState
{
    public MapTerrainType Terrain { get; private set; }
    public int Temperature { get; private set; }
    public bool Hill { get; private set; }
    public RiverState River { get; private set; } = new RiverState();


    private readonly WorldmapCell cell;
    public WorldmapState(WorldmapCell cell)
    {
        this.cell = cell;
    }

    internal void Apply(CardType model)
    {
        UnityEngine.Debug.Log(model.ToString() + " dropped on " + cell.X + " " + cell.Y);
    }
}

public enum MapTerrainType
{
    Plains,
    Grassland,
    Forest,
    Savannah,
    Jungle,
    Tundra,
    Desert,
    Sea,
    Coast,
    Lake,
    Mountain,
    Oasis,
    Swamp,
    Wetland
}

public class RiverState
{
    public bool Exists
    {
        get
        {
            return ConnectsNorth || ConnectsSouth || ConnectsEast || ConnectsWest;
        }
    }
    public bool ConnectsNorth { get; }
    public bool ConnectsSouth { get; }
    public bool ConnectsEast { get; }
    public bool ConnectsWest { get; }

    public RiverState(bool connectsNorth = false, bool connectsSouth = false, bool connectsEast = false, bool connectsWest = false)
    {
        ConnectsNorth = connectsNorth;
        ConnectsSouth = connectsSouth;
        ConnectsEast = connectsEast;
        ConnectsWest = connectsWest;
    }
}
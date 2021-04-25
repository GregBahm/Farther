using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum SiteType
{
    None = 0,
    DragonLair,
    ForestVillage,
    MagicalBathouse,
    City
}

public class TerrainState
{
    public MapTerrainType Terrain { get; }
    public int Temperature { get; }
    public bool Hill { get; }
    public bool River { get; }

    public TerrainState(
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
    public TerrainStateBuilder ToBuilder()
    {
        return new TerrainStateBuilder(this);
    }
}

public struct StateChangeResult
{
    public bool StateCanChange { get; }
    public WorldmapState NewState { get; }

    public StateChangeResult(bool canChange = false, WorldmapState newState = null)
    {
        StateCanChange = canChange;
        NewState = newState;
    }
}

public class TerrainStateBuilder
{
    public MapTerrainType Terrain { get; set; }
    public int Temperature { get; set; }
    public bool Hill { get; set; }
    public bool River { get; set; }

    public TerrainStateBuilder(TerrainState sourceState)
    {
        Terrain = sourceState.Terrain;
        Temperature = sourceState.Temperature;
        Hill = sourceState.Hill;
        River = sourceState.River;
    }

    public TerrainState ToState()
    {
        return new TerrainState(Terrain,
            Temperature,
            Hill,
            River
            );
    }
}

public class SitelessTile : WorldmapState
{
    public SitelessTile(TerrainState terrain)
        : base(terrain, SiteType.None) 
    { }

    protected override IEnumerable<DropRecipe> GetDropRecipes()
    {
        yield return EarthOnVoidToPlains;
    }

    protected override IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes()
    {
        return new PassiveRecipe[0];
        // TODO:
        // Tallest Mountain recipe
        // Oasis
        // Sea to Coast
    }

    private StateChangeResult EarthOnVoidToPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Earth && Terrain.Terrain == MapTerrainType.Void;
        SitelessTile newState = null;
        if(canDrop)
        {
            newState = GetEarthFromVoidToPlains();
        }
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetEarthFromVoidToPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature < 0)
            newTerrain.Terrain = MapTerrainType.Tundra;
        else if (Terrain.Temperature > 0)
            newTerrain.Terrain = MapTerrainType.Desert;
        else
            newTerrain.Terrain = MapTerrainType.Plains;

        return  new SitelessTile(newTerrain.ToState());

    }
}

public class Card
{
    public CardType Type { get; }
}
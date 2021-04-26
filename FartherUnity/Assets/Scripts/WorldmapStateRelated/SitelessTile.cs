using System;
using System.Collections.Generic;

public class SitelessTile : WorldmapState
{
    public SitelessTile(TerrainState terrain)
        : base(terrain, SiteType.None) 
    { }

    protected override IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes()
    {
        return new PassiveRecipe[0];
        // TODO:
        // Tallest Mountain recipe
        // Oasis
        // Sea to Coast
    }

    protected override IEnumerable<DropRecipe> GetDropRecipes()
    {
        yield return EarthOnVoid;
        yield return GreeneryOnPlains;
        yield return GreeneryOnGreenery;
        yield return FloodOnForest;
        yield return FloodOnPlains;
        yield return FloodOnVoid;
        yield return EarthOnLand;
    }

    private static readonly HashSet<MapTerrainType> hillTypes =
        new HashSet<MapTerrainType>() {
                MapTerrainType.Desert,
                MapTerrainType.Forest,
                MapTerrainType.Grassland,
                MapTerrainType.Jungle,
                MapTerrainType.Plains,
                MapTerrainType.Savannah,
                MapTerrainType.Tundra,
        };

    private StateChangeResult EarthOnLand(Card card)
    {
        bool canDrop = card.Type == CardType.Earth
            && hillTypes.Contains(Terrain.Type);
        SitelessTile newState = canDrop ? GetEarthOnLand() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetEarthOnLand()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if(Terrain.Hill)
        {
            newTerrain.Terrain = MapTerrainType.Mountain;
        }else
        {
            newTerrain.Hill = true;
        }
        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult FloodOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Flood
            && Terrain.Type == MapTerrainType.Void;
        SitelessTile newState = canDrop ? GetFloodOnVoid() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetFloodOnVoid()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Terrain = MapTerrainType.Sea;
        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult FloodOnPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Flood
            && Terrain.Type == MapTerrainType.Plains
            && !Terrain.Hill;
        SitelessTile newState = canDrop ? GetFloodOnPlains() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetFloodOnPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Terrain = MapTerrainType.Wetland;
        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult FloodOnForest(Card card)
    {
        bool canDrop = card.Type == CardType.Flood
            && Terrain.Type == MapTerrainType.Forest
            && !Terrain.Hill;
        SitelessTile newState = canDrop ? GetFloodOnForest() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetFloodOnForest()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Terrain = MapTerrainType.Swamp;
        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult GreeneryOnGreenery(Card card)
    {
        bool canDrop = card.Type == CardType.Greenery
            && (Terrain.Type == MapTerrainType.Grassland
            || Terrain.Type == MapTerrainType.Savannah);
        SitelessTile state = canDrop ? GetGreeneryOnGreenery() : null;
        return new StateChangeResult(canDrop, state);
    }

    private SitelessTile GetGreeneryOnGreenery()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature > 0)
            newTerrain.Terrain = MapTerrainType.Jungle;
        else
            newTerrain.Terrain = MapTerrainType.Forest;

        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult GreeneryOnPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Greenery &&
            (Terrain.Type == MapTerrainType.Plains
            || Terrain.Type == MapTerrainType.Desert);
        SitelessTile newState = canDrop ? GetGreeneryOnPlains() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetGreeneryOnPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature < 0)
            newTerrain.Terrain = MapTerrainType.Savannah;
        else
            newTerrain.Terrain = MapTerrainType.Grassland;

        return new SitelessTile(newTerrain.ToState());
    }

    private StateChangeResult EarthOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Earth && Terrain.Type == MapTerrainType.Void;
        SitelessTile newState = canDrop ? GetEarthOnVoid() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessTile GetEarthOnVoid()
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

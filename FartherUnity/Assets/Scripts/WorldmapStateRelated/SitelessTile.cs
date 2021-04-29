using System;
using System.Collections.Generic;
using System.Linq;

public class SitelessTile : WorldmapState
{
    public SitelessTile(WorldmapPosition position, TerrainState terrain)
        : base(position, terrain, SiteType.None) 
    { }

    protected override IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes()
    {
        yield return SeaToCoast;
        // TODO:
        // Tallest Mountain recipe
        // Oasis
        // Sea to Coast
    }

    private static readonly HashSet<MapTerrainType> seaTypes =
        new HashSet<MapTerrainType>() {
                MapTerrainType.Sea,
                MapTerrainType.Coast,
        };


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

    private StateChangeResult SeaToCoast()
    {
        bool shouldChange = GetShouldChangeSeaToCoast();
        SitelessTile newState = shouldChange ? GetSeaToCoast() : null;
        return new StateChangeResult(shouldChange, newState);
    }

    private SitelessTile GetSeaToCoast()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Coast;
        return new SitelessTile(Position, newTerrain.ToState());
    }

    private bool GetShouldChangeSeaToCoast()
    {
        if (Terrain.Type != MapTerrainType.Sea)
            return false;
        return Position.Neighbors.Select(item => item.State.Terrain.Type).Any(item => IsLand(item));
    }

    private bool IsLand(MapTerrainType item)
    {
        return !seaTypes.Contains(item) && item != MapTerrainType.Void;
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
            newTerrain.Type = MapTerrainType.Mountain;
        }else
        {
            newTerrain.Hill = true;
        }
        return new SitelessTile(Position, newTerrain.ToState());
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
        newTerrain.Type = MapTerrainType.Sea;
        return new SitelessTile(Position, newTerrain.ToState());
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
        newTerrain.Type = MapTerrainType.Wetland;
        return new SitelessTile(Position, newTerrain.ToState());
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
        newTerrain.Type = MapTerrainType.Swamp;
        return new SitelessTile(Position, newTerrain.ToState());
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
            newTerrain.Type = MapTerrainType.Jungle;
        else
            newTerrain.Type = MapTerrainType.Forest;

        return new SitelessTile(Position, newTerrain.ToState());
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
            newTerrain.Type = MapTerrainType.Savannah;
        else
            newTerrain.Type = MapTerrainType.Grassland;

        return new SitelessTile(Position, newTerrain.ToState());
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
            newTerrain.Type = MapTerrainType.Tundra;
        else if (Terrain.Temperature > 0)
            newTerrain.Type = MapTerrainType.Desert;
        else
            newTerrain.Type = MapTerrainType.Plains;

        return  new SitelessTile(Position, newTerrain.ToState());

    }
}

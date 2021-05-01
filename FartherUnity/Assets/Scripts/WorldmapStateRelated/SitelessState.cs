using System;
using System.Collections.Generic;
using System.Linq;

public class SitelessState : WorldmapState
{
    public SitelessState(WorldmapPosition position, TerrainState terrain)
        : base(position, terrain, SiteType.None) 
    { }

    protected override IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes()
    {
        yield return SeaToCoast;
        yield return CoastToLake;
        yield return LakeToOasis;
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
        yield return DragonLair;
    }

    private StateChangeResult DragonLair(Card card)
    {
        bool shouldChange = card.Type == CardType.Wilds
            && Terrain.Mythic
            && Terrain.Type == MapTerrainType.Mountain;
        WorldmapState dragonState = new DragonLairState(Position, Terrain, SiteType.DragonLair);
        return new StateChangeResult(shouldChange, dragonState);
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

    private StateChangeResult LakeToOasis()
    {
        bool shouldChange = GetShouldChangeLakeToOasis();
        SitelessState newState = shouldChange ? GetLakeToOasis() : null;
        return new StateChangeResult(shouldChange, newState);
    }

    private SitelessState GetLakeToOasis()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Oasis;
        return new SitelessState(Position, newTerrain.ToState());
    }

    private bool GetShouldChangeLakeToOasis()
    {
        if(Terrain.Type == MapTerrainType.Lake)
        {
            return Position.Neighbors.All(item => item.State.Terrain.Type == MapTerrainType.Desert);
        }
        return false;
    }

    // Coast turns to lake if it is surrounded by land or coast tiles that are also surrounded by land
    private StateChangeResult CoastToLake()
    {
        bool shouldChange = GetShouldChangeCoastToLake();
        SitelessState newState = shouldChange ? GetCoastToLake() : null;
        return new StateChangeResult(shouldChange, newState);
    }

    private SitelessState GetCoastToLake()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Lake;
        return new SitelessState(Position, newTerrain.ToState());
    }

    private bool GetShouldChangeCoastToLake()
    {
        if (Terrain.Type != MapTerrainType.Coast)
            return false;

        return Position.Neighbors.All(QualifiesForLake);
    }

    // A lake must be surrounded by either land, or coast surrounded by land and coast.
    private bool QualifiesForLake(WorldmapPosition neighbor)
    {
        if (IsLand(neighbor.State.Terrain.Type))
            return true;

        foreach (MapTerrainType terrainType in neighbor.Neighbors.Select(item => item.State.Terrain.Type))
        {
            if(!IsLand(terrainType) && terrainType != MapTerrainType.Coast)
                return false;
        }
        return true;
    }

    // Sea turns to coast if it is touching any land
    private StateChangeResult SeaToCoast()
    {
        bool shouldChange = GetShouldChangeSeaToCoast();
        SitelessState newState = shouldChange ? GetSeaToCoast() : null;
        return new StateChangeResult(shouldChange, newState);
    }

    private SitelessState GetSeaToCoast()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Coast;
        return new SitelessState(Position, newTerrain.ToState());
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

    private StateChangeResult EarthOnLand(Card card)
    {
        bool canDrop = card.Type == CardType.Earth
            && hillTypes.Contains(Terrain.Type);
        SitelessState newState = canDrop ? GetEarthOnLand() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetEarthOnLand()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if(Terrain.Hill)
        {
            newTerrain.Type = MapTerrainType.Mountain;
        }else
        {
            newTerrain.Hill = true;
        }
        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult FloodOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Depths
            && Terrain.Type == MapTerrainType.Void;
        SitelessState newState = canDrop ? GetFloodOnVoid() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetFloodOnVoid()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Sea;
        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult FloodOnPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Depths
            && Terrain.Type == MapTerrainType.Plains
            && !Terrain.Hill;
        SitelessState newState = canDrop ? GetFloodOnPlains() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetFloodOnPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Wetland;
        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult FloodOnForest(Card card)
    {
        bool canDrop = card.Type == CardType.Depths
            && Terrain.Type == MapTerrainType.Forest
            && !Terrain.Hill;
        SitelessState newState = canDrop ? GetFloodOnForest() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetFloodOnForest()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Swamp;
        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult GreeneryOnGreenery(Card card)
    {
        bool canDrop = card.Type == CardType.Greenery
            && (Terrain.Type == MapTerrainType.Grassland
            || Terrain.Type == MapTerrainType.Savannah);
        SitelessState state = canDrop ? GetGreeneryOnGreenery() : null;
        return new StateChangeResult(canDrop, state);
    }

    private SitelessState GetGreeneryOnGreenery()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature > 0)
            newTerrain.Type = MapTerrainType.Jungle;
        else
            newTerrain.Type = MapTerrainType.Forest;

        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult GreeneryOnPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Greenery &&
            (Terrain.Type == MapTerrainType.Plains
            || Terrain.Type == MapTerrainType.Desert);
        SitelessState newState = canDrop ? GetGreeneryOnPlains() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetGreeneryOnPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature < 0)
            newTerrain.Type = MapTerrainType.Savannah;
        else
            newTerrain.Type = MapTerrainType.Grassland;

        return new SitelessState(Position, newTerrain.ToState());
    }

    private StateChangeResult EarthOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Earth && Terrain.Type == MapTerrainType.Void;
        SitelessState newState = canDrop ? GetEarthOnVoid() : null;
        return new StateChangeResult(canDrop, newState);
    }

    private SitelessState GetEarthOnVoid()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature < 0)
            newTerrain.Type = MapTerrainType.Tundra;
        else if (Terrain.Temperature > 0)
            newTerrain.Type = MapTerrainType.Desert;
        else
            newTerrain.Type = MapTerrainType.Plains;

        return  new SitelessState(Position, newTerrain.ToState());

    }
}

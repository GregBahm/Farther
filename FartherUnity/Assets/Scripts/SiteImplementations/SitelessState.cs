using System;
using System.Collections.Generic;
using System.Linq;

public class SitelessState : MapCellState
{
    public SitelessState(MapCell cell, TerrainState terrain)
        : base(cell, terrain, SiteType.None) 
    { }

    protected override IEnumerable<Effector> GetOnNeighborChangeEffectors()
    {
        yield return SeaToCoast;
        yield return CoastToLake;
        yield return LakeToOasis;
    }

    protected override IEnumerable<CardDropEffector> GetDropMutators()
    {
        yield return EarthOnVoid;
        yield return PlantsOnPlains;
        yield return PlantsOnPlants;
        yield return WaterOnForest;
        yield return WaterOnGrassland;
        yield return WaterOnVoid;
        yield return EarthOnLand;
        yield return DragonLair;
    }

    private EffectorResult DragonLair(Card card)
    {
        bool shouldChange = card.Type == CardType.Wilds
            && Terrain.Mythic
            && Terrain.Type == MapTerrainType.Mountain;
        if (shouldChange)
            return new EffectorResult(new DragonLairState(Cell, Terrain));
        return EffectorResult.NoEffect;
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

    private EffectorResult LakeToOasis()
    {
        bool shouldChange = GetShouldChangeLakeToOasis();
        if (shouldChange)
            return new EffectorResult(GetLakeToOasis());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetLakeToOasis()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Oasis;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private bool GetShouldChangeLakeToOasis()
    {
        if(Terrain.Type == MapTerrainType.Lake)
        {
            return Cell.Neighbors.All(item => item.State.Terrain.Type == MapTerrainType.Desert);
        }
        return false;
    }

    // Coast turns to lake if it is surrounded by land or coast tiles that are also surrounded by land
    private EffectorResult CoastToLake()
    {
        bool shouldChange = GetShouldChangeCoastToLake();
        if (shouldChange)
            return new EffectorResult(GetCoastToLake());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetCoastToLake()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Lake;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private bool GetShouldChangeCoastToLake()
    {
        if (Terrain.Type != MapTerrainType.Coast)
            return false;

        return Cell.Neighbors.All(QualifiesForLake);
    }

    // A lake must be surrounded by either land, or coast surrounded by land and coast.
    private bool QualifiesForLake(MapCell neighbor)
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
    private EffectorResult SeaToCoast()
    {
        bool shouldChange = GetShouldChangeSeaToCoast();
        if (shouldChange)
            return new EffectorResult(GetSeaToCoast());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetSeaToCoast()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Coast;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private bool GetShouldChangeSeaToCoast()
    {
        if (Terrain.Type != MapTerrainType.Sea)
            return false;
        return Cell.Neighbors.Select(item => item.State.Terrain.Type).Any(item => IsLand(item));
    }

    private bool IsLand(MapTerrainType item)
    {
        return !seaTypes.Contains(item) && item != MapTerrainType.Void;
    }

    private EffectorResult EarthOnLand(Card card)
    {
        bool canDrop = card.Type == CardType.Earth
            && hillTypes.Contains(Terrain.Type);
        if (canDrop)
            return new EffectorResult(GetEarthOnLand());
        return EffectorResult.NoEffect;
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
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult WaterOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Water
            && Terrain.Type == MapTerrainType.Void;
        if (canDrop)
            return new EffectorResult(GetWaterOnVoid());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetWaterOnVoid()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Sea;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult WaterOnGrassland(Card card)
    {
        bool canDrop = card.Type == CardType.Water
            && Terrain.Type == MapTerrainType.Grassland
            && !Terrain.Hill;
        if (canDrop)
            return new EffectorResult(GetWaterOnGrassland());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetWaterOnGrassland()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Wetland;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult WaterOnForest(Card card)
    {
        bool canDrop = card.Type == CardType.Water
            && Terrain.Type == MapTerrainType.Forest
            && !Terrain.Hill;
        if (canDrop)
            return new EffectorResult(GetWaterOnForest());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetWaterOnForest()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        newTerrain.Type = MapTerrainType.Swamp;
        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult PlantsOnPlants(Card card)
    {
        bool canDrop = card.Type == CardType.Plants
            && (Terrain.Type == MapTerrainType.Grassland
            || Terrain.Type == MapTerrainType.Savannah);
        if (canDrop)
            return new EffectorResult(GetPlantsOnPlants());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetPlantsOnPlants()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature > 0)
            newTerrain.Type = MapTerrainType.Jungle;
        else
            newTerrain.Type = MapTerrainType.Forest;

        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult PlantsOnPlains(Card card)
    {
        bool canDrop = card.Type == CardType.Plants &&
            (Terrain.Type == MapTerrainType.Plains
            || Terrain.Type == MapTerrainType.Desert);
        if (canDrop)
            return new EffectorResult(GetPlantsOnPlains());
        return EffectorResult.NoEffect;
    }

    private SitelessState GetPlantsOnPlains()
    {
        TerrainStateBuilder newTerrain = Terrain.ToBuilder();
        if (Terrain.Temperature < 0)
            newTerrain.Type = MapTerrainType.Savannah;
        else
            newTerrain.Type = MapTerrainType.Grassland;

        return new SitelessState(Cell, newTerrain.ToState());
    }

    private EffectorResult EarthOnVoid(Card card)
    {
        bool canDrop = card.Type == CardType.Earth && Terrain.Type == MapTerrainType.Void;
        if (canDrop)
            return new EffectorResult(GetEarthOnVoid());
        return EffectorResult.NoEffect;
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

        return  new SitelessState(Cell, newTerrain.ToState());

    }
}

using System.Collections.Generic;

public abstract class PassiveRecipe
{
    public abstract Dictionary<WorldmapSlot, WorldmapState> GetModifiedCells(Worldmap worldMap);


    private static readonly HashSet<MapTerrainType> waterTypes = new HashSet<MapTerrainType>()
    {
        MapTerrainType.Coast,
        MapTerrainType.Lake,
        MapTerrainType.Sea,
        MapTerrainType.Oasis,
    };

    public static bool IsWaterType(MapTerrainType type)
    {
        return waterTypes.Contains(type);
    }
}
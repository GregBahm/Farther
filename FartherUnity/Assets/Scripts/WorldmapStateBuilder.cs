public class WorldmapStateBuilder
{
    public MapTerrainType Terrain { get; set; }
    public int Temperature { get; set; }
    public bool Hill { get; set; }
    public bool River { get; set; }

    public WorldmapStateBuilder(WorldmapState sourceState)
    {
        Terrain = sourceState.Terrain;
        Temperature = sourceState.Temperature;
        Hill = sourceState.Hill;
        River = sourceState.River;
    }

    public WorldmapState ToState()
    {
        return new WorldmapState(Terrain,
            Temperature,
            Hill,
            River
            );
    }
}

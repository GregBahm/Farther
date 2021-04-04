public class WorldmapStateBuilder
{
    public MapTerrainType Terrain { get; set; }
    public int Temperature { get; set; }
    public bool Hill { get; set; }
    public RiverStateBuilder River { get; }

    public WorldmapStateBuilder(WorldmapState sourceState)
    {
        Terrain = sourceState.Terrain;
        Temperature = sourceState.Temperature;
        Hill = sourceState.Hill;

    }

    public WorldmapState ToState()
    {
        return new WorldmapState(Terrain,
            Temperature,
            Hill,
            River.ToState()
            );
    }
}

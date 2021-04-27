public class TerrainStateBuilder
{
    public MapTerrainType Type { get; set; }
    public int Temperature { get; set; }
    public bool Hill { get; set; }
    public bool River { get; set; }

    public TerrainStateBuilder(TerrainState sourceState)
    {
        Type = sourceState.Type;
        Temperature = sourceState.Temperature;
        Hill = sourceState.Hill;
        River = sourceState.River;
    }

    public TerrainState ToState()
    {
        return new TerrainState(Type,
            Temperature,
            Hill,
            River
            );
    }
}

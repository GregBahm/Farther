public class TerrainState
{
    public MapTerrainType Type { get; }
    public int Temperature { get; }
    public bool Hill { get; }
    public bool River { get; }

    public TerrainState(
        MapTerrainType type = default,
        int temperature = 0,
        bool hill = false,
        bool river = false)
    {
        Type = type;
        Temperature = temperature;
        Hill = hill;
        River = river;
    }
    public TerrainStateBuilder ToBuilder()
    {
        return new TerrainStateBuilder(this);
    }
}

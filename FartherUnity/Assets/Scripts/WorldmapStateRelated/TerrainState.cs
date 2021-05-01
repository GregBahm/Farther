public class TerrainState
{
    public MapTerrainType Type { get; }
    public int Temperature { get; }
    public bool Hill { get; }
    public bool River { get; }
    public bool Mythic { get; }

    public TerrainState(
        MapTerrainType type = default,
        int temperature = 0,
        bool hill = false,
        bool river = false,
        bool mythic = false)
    {
        Type = type;
        Temperature = temperature;
        Hill = hill;
        River = river;
        Mythic = mythic;
    }
    public TerrainStateBuilder ToBuilder()
    {
        return new TerrainStateBuilder(this);
    }
}

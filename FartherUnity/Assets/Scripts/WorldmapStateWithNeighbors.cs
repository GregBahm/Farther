using System.Collections.Generic;

public class WorldmapStateWithNeighbors
{
    public WorldmapState Center { get; }
    public IEnumerable<WorldmapState> Neighbors { get; }

    public WorldmapStateWithNeighbors(WorldmapState center, IEnumerable<WorldmapState> neighors)
    {
        Center = center;
        Neighbors = neighors;
    }
}

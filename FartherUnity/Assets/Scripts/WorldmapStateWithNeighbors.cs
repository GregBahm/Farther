using System.Collections.Generic;

public class WorldmapStateWithNeighbors
{
    public WorldmapState Center { get; }
    public Neighbors<WorldmapState> Neighbors { get; }

    public WorldmapStateWithNeighbors(WorldmapState center, Neighbors<WorldmapState> neighors)
    {
        Center = center;
        Neighbors = neighors;
    }
}

public class Worldmap
{
    public WorldmapCell[,] Cells { get; }

    public int Width { get; }
    public int Height { get; }

    public Worldmap(int width,
        int height)
    {
        Width = width;
        Height = height;
        Cells = Initialize();
    }

    private WorldmapCell[,] Initialize()
    {
        WorldmapCell[,] ret = new WorldmapCell[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ret[x, y] = new WorldmapCell(x, y, this);
            }
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                ret[x, y].InitializeNeighbors(ret);
            }
        }
        return ret;
    }
}

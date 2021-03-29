using System;
using System.Diagnostics;

public class WorldmapState
{
    private readonly WorldmapCell cell;
    public WorldmapState(WorldmapCell cell)
    {
        this.cell = cell;
    }

    internal void Apply(CardType model)
    {
        UnityEngine.Debug.Log(model.ToString() + " dropped on " + cell.X + " " + cell.Y);
    }
}

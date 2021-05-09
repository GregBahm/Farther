using System;

public class Game
{
    public Map Map { get; }
    
    public Cards Cards { get; }

    public static event EventHandler TurnEnd;

    public Game()
    {
        Map = new Map(this);
    }

    public void AdvanceTurn()
    {
        TurnEnd?.Invoke(this, EventArgs.Empty);
    }

    public static Game Load(string path)
    {
        throw new NotImplementedException();
    }

    public void Save(string path)
    {
        throw new NotImplementedException();
    }
}

using System;

public class GameState
{
    public Worldmap Map { get; }
    
    public CardsState Cards { get; }

    public static event EventHandler TurnEnd;

    public GameState()
    {
        Map = new Worldmap(this);
    }

    public void AdvanceTurn()
    {
        TurnEnd?.Invoke(this, EventArgs.Empty);
    }

    public static GameState Load(string path)
    {
        throw new NotImplementedException();
    }

    public void Save(string path)
    {
        throw new NotImplementedException();
    }
}

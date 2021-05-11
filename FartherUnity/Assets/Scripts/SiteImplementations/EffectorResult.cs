using System.Collections.Generic;
using System.Linq;

public class EffectorResult
{
    public static EffectorResult NoEffect { get; }

    private static readonly IEnumerable<MapCellState> NoStateChanges;
    private static readonly IEnumerable<Card> NoNewCards;

    static EffectorResult()
    {
        NoStateChanges = new MapCellState[0];
        NoNewCards = new Card[0];
        NoEffect = new EffectorResult();
    }

    public bool AnyEffect { get; }

    public IEnumerable<MapCellState> NewStates { get; }
    public IEnumerable<Card> NewCards { get; }

    public EffectorResult(MapCellState newState)
        :this(new MapCellState[] { newState })
    { }
    public EffectorResult(IEnumerable<MapCellState> newStates = null,
        IEnumerable<Card> newCards = null)
    {
        NewStates = newStates ?? NoStateChanges; 
        NewCards = newCards ?? NoNewCards;
        
        AnyEffect = NewStates.Any() || NewCards.Any();
    }

    public void ApplyEffect(Game game)
    {
        foreach (MapCellState item in NewStates)
        {
            item.Cell.State = item;
        }
        game.Cards.Add(NewCards);
    }
}

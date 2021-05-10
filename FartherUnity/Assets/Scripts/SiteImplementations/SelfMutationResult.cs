using System.Collections.Generic;

public struct SelfMutationResult
{
    private static readonly IEnumerable<Card> NoCards = new Card[0];

    public bool StateChanged { get; }
    public MapCellState NewState { get; }

    private readonly IEnumerable<Card> gainedCards;
    public IEnumerable<Card> GainedCards
    {
        get
        {
            return gainedCards ?? NoCards;
        }
    }

    public SelfMutationResult(bool stateChanged = false, 
        MapCellState newState = null,
        IEnumerable<Card> gainedCards = null)
    {
        StateChanged = stateChanged;
        NewState = newState;
        this.gainedCards = gainedCards;
    }
}

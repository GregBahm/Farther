using System.Collections.Generic;

public struct SelfMutationResult
{
    public bool StateChanged { get; }
    public MapCellState NewState { get; }

    public IEnumerable<Card> GainedCards { get; }

    public SelfMutationResult(bool stateChanged = false, 
        MapCellState newState = null,
        IEnumerable<Card> gainedCards = null)
    {
        StateChanged = stateChanged;
        NewState = newState;
        GainedCards = gainedCards;
    }
}

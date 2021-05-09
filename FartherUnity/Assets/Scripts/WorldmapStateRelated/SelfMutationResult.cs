using System.Collections.Generic;

public struct SelfMutationResult
{
    public bool StateChanged { get; }
    public WorldmapState NewState { get; }

    public IEnumerable<Card> GainedCards { get; }

    public SelfMutationResult(bool stateChanged = false, 
        WorldmapState newState = null,
        IEnumerable<Card> gainedCards = null)
    {
        StateChanged = stateChanged;
        NewState = newState;
        GainedCards = gainedCards;
    }
}

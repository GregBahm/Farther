public struct StateChangeResult
{
    public bool StateCanChange { get; }
    public WorldmapState NewState { get; }

    public StateChangeResult(bool canChange = false, WorldmapState newState = null)
    {
        StateCanChange = canChange;
        NewState = newState;
    }
}

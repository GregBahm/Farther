public abstract class CardDropRecipe
{
    public abstract CardType Card { get; }
    public abstract bool CanModifyState(WorldmapStateWithNeighbors state);
    public abstract WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState);
}

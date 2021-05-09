using System.Linq;

public struct TargetedMutationResult
{
    public bool StatesChanged { get { return Targets.Any(); } }

    public MutationTarget[] Targets { get; }

    public TargetedMutationResult(params MutationTarget[] targts)
    {
        Targets = targts ?? new MutationTarget[0];
    }
}

public struct MutationTarget
{
    public MapCellState NewState { get; }
    public MapCell TargetPosition { get; }

    public MutationTarget(MapCellState newState, MapCell targetPosition)
    {
        NewState = newState;
        TargetPosition = targetPosition;
    }

}
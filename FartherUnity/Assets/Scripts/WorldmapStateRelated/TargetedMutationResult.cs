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
    public WorldmapState NewState { get; }
    public WorldmapPosition TargetPosition { get; }

    public MutationTarget(WorldmapState newState, WorldmapPosition targetPosition)
    {
        NewState = newState;
        TargetPosition = targetPosition;
    }

}
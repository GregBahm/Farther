using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DragonLairState : WorldmapState
{
    public readonly int currentDragonAttitude;
    public const int MaxHostility = -5;
    public const int PacificationThreshold = 1;
    public const int AllyThreshold = 5;
    public const int TurnsSinceTributeLimit = 5;

    public DragonDisposition Disposition { get; }

    public int TurnsSinceTribute { get; }

    public enum DragonDisposition
    {
        Hostile,
        Pacified,
        Ally
    }

    public DragonLairState(WorldmapPosition position, 
        TerrainState terrain,
        int currentDragonAttitude,
        int turnsSinceTribute) 
        : base(position, terrain, SiteType.DragonLair)
    {
        this.currentDragonAttitude = currentDragonAttitude;
        TurnsSinceTribute = turnsSinceTribute;
        Disposition = GetDisposition();
    }

    private DragonDisposition GetDisposition()
    {
        if (currentDragonAttitude >= AllyThreshold)
            return DragonDisposition.Ally;
        if (currentDragonAttitude >= PacificationThreshold)
            return DragonDisposition.Pacified;
        return DragonDisposition.Hostile;
    }

    protected override IEnumerable<CardDropMutator> GetDropMutators()
    {
        yield return TributeDropped;
        yield return WarriorDropped;
    }

    private SelfMutationResult WarriorDropped(Card card)
    {
        // Need to figure out the warrior's level.
        // So I guess I'll need to cast the warrior to a warrior card and get its level.
        // Then I'll need to set up Mutations to allow drops, in case the warrior slays the dragon.
        // And I'll also need to potentially give back a warrior card upgraded or injured.
        // So that will require expanding the card and card management aspect of gamestate
        throw new NotImplementedException();
    }

    protected override IEnumerable<PassiveTargetedMutator> GetOnTurnEndMutators()
    {
        return base.GetOnTurnEndMutators();
        // TODO need to set up an update that drains a timer
        // And then when the timer falls, have the dragon start raiding
        // And then after raiding, grow in menace
    }

    private SelfMutationResult TributeDropped(Card card)
    {
        bool canDrop = card.Type == CardType.Wealth && Disposition != DragonDisposition.Ally;
        DragonLairState newState = canDrop ? GetOnTributeDropped() : null;
        return new SelfMutationResult(canDrop, newState);
    }

    private DragonLairState GetOnTributeDropped()
    {
        return new DragonLairState(Position, Terrain, currentDragonAttitude + 1, 0);
    }
}

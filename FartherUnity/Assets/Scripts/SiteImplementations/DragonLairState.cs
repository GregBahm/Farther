using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DragonLairState : MapCellState
{
    public const int MaxHostility = -5;
    public const int PacificationThreshold = 1;
    public const int AllyThreshold = 5;
    public const int TurnsSinceTributeLimit = 5;

    public DragonDisposition Disposition { get; }

    public int Attitude { get; }

    public int TurnsSinceTribute { get; }

    public int Level { get; }

    public enum DragonDisposition
    {
        Hostile,
        Pacified,
        Ally
    }

    public DragonLairState(MapCell position, 
        TerrainState terrain,
        int level = 0,
        int attitude = 0,
        int turnsSinceTribute = 0) 
        : base(position, terrain, SiteType.DragonLair)
    {
        Level = level;
        Attitude = attitude;
        TurnsSinceTribute = turnsSinceTribute;
        Disposition = GetDisposition();
    }

    private DragonDisposition GetDisposition()
    {
        if (Attitude >= AllyThreshold)
            return DragonDisposition.Ally;
        if (Attitude >= PacificationThreshold)
            return DragonDisposition.Pacified;
        return DragonDisposition.Hostile;
    }

    protected override IEnumerable<CardDropEffector> GetDropMutators()
    {
        yield return TributeDropped;
        yield return WarriorDropped;
    }

    private EffectorResult WarriorDropped(Card card)
    {
        if(card.Type != CardType.Warrior)
        {
            return EffectorResult.NoEffect;
        }
        WarriorCard warriorCard = card as WarriorCard;
        throw new NotImplementedException();
    }

    protected override IEnumerable<Effector> GetOnTurnEndEffectors()
    {
        yield return NextTurnDragon;
        // TODO need to set up an update that drains a timer
        // And then when the timer falls, have the dragon start raiding
        // And then after raiding, grow in menace
    }

    private EffectorResult NextTurnDragon()
    {
        DragonLairStateBuilder builder = ToBuilder();
        builder.Attitude += 1;
        return new EffectorResult(builder.ToState());
    }

    private EffectorResult TributeDropped(Card card)
    {
        bool canDrop = card.Type == CardType.Wealth && Disposition != DragonDisposition.Ally;
        DragonLairState newState = canDrop ? GetOnTributeDropped() : null;
        return new EffectorResult(newState);
    }

    private DragonLairState GetOnTributeDropped()
    {
        DragonLairStateBuilder builder = ToBuilder();
        builder.Attitude += 1;
        return builder.ToState();
    }

    private class DragonLairStateBuilder
    {
        private readonly DragonLairState originalState;

        public int Attitude { get; set; }

        public int TurnsSinceTribute { get; set; }

        public int Level { get; set; }

        public DragonLairStateBuilder(DragonLairState originalState)
        {
            this.originalState = originalState;
            Attitude = originalState.Attitude;
            TurnsSinceTribute = originalState.TurnsSinceTribute;
            Level = originalState.Level;
        }

        public DragonLairState ToState()
        {
            return new DragonLairState(originalState.Cell, 
                originalState.Terrain, 
                Level, 
                Attitude, 
                TurnsSinceTribute);
        }
    }

    private DragonLairStateBuilder ToBuilder()
    {
        return new DragonLairStateBuilder(this);
    }
}

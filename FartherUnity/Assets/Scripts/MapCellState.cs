using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

public abstract class MapCellState
{
    protected MapCell Cell { get; }

    public TerrainState Terrain { get; }

    public SiteType SiteType { get; }

    public Game Game { get { return Cell.Map.Game; } }

    protected delegate SelfMutationResult CardDropMutator(Card card);

    protected delegate SelfMutationResult PassiveSelfMutator();

    protected delegate TargetedMutationResult PassiveTargetedMutator();

    // Evaluated when a card is dropped on the worldmap slot
    private readonly IEnumerable<CardDropMutator> dropMutators;

    // Evaluated when the state of a neighboring slot changes
    private readonly IEnumerable<PassiveSelfMutator> onNeighborChangeMutators;

    // Evaluated when a turn ends
    private readonly IEnumerable<PassiveTargetedMutator> onTurnEndMutators;

    public MapCellState(MapCell cell,
        TerrainState terrain,
        SiteType siteType)
    {
        Cell = cell;
        Terrain = terrain;
        SiteType = siteType;
        dropMutators = GetDropMutators().ToArray();
        onNeighborChangeMutators = GetOnNeighborChangeMutators().ToArray();
        onTurnEndMutators = GetOnTurnEndMutators().ToArray();
    }

    public bool CanDropCardOnTile(Card card)
    {
        return dropMutators.Any(item => item(card).StateChanged);
    }

    protected virtual IEnumerable<CardDropMutator> GetDropMutators()
    {
        return new CardDropMutator[0];
    }

    protected virtual IEnumerable<PassiveSelfMutator> GetOnNeighborChangeMutators()
    {
        return new PassiveSelfMutator[0];
    }

    protected virtual IEnumerable<PassiveTargetedMutator> GetOnTurnEndMutators()
    {
        return new PassiveTargetedMutator[0];
    }

    public MapCellState GetFromDrop(Card card)
    {
        foreach (CardDropMutator item in dropMutators)
        {
            SelfMutationResult result = item(card);
            if (result.StateChanged)
            {
                return result.NewState;
            }
        }
        throw new InvalidOperationException("Can't GetFromDrop when no mutators can drop.");
    }

    private readonly List<EventHandler> stateChangeListeners = new List<EventHandler>();
    private readonly List<EventHandler> turnEndListeners = new List<EventHandler>();

    internal void OnRemovedFromMap()
    {
        foreach (EventHandler listener in turnEndListeners)
        {
            Game.TurnEnd -= listener;
        }

        foreach (MapCell neighbor in Cell.Neighbors)
        {
            foreach (EventHandler listener in stateChangeListeners)
            {
                neighbor.StateChanged -= listener;
            }
        }
    }

    internal void OnAddedToMap()
    {
        foreach (PassiveTargetedMutator turnEndMutator in onTurnEndMutators)
        {
            EventHandler action = (sender, e) => ProcessTurnEndMutators(turnEndMutator);
            Game.TurnEnd += action;
            turnEndListeners.Add(action);
        }

        foreach (MapCell neighbor in Cell.Neighbors)
        {
            foreach (PassiveSelfMutator passiveMutator in onNeighborChangeMutators)
            {
                EventHandler action = (sender, e) => ProcessPassiveMutator(passiveMutator);
                neighbor.StateChanged += action;
                stateChangeListeners.Add(action);
            }
        }

        foreach(PassiveSelfMutator mutator in onNeighborChangeMutators)
        {
            ProcessPassiveMutator(mutator);
        }
    }

    private void ProcessTurnEndMutators(PassiveTargetedMutator mutator)
    {
        TargetedMutationResult result = mutator();
        if(result.StatesChanged)
        {
            foreach (MutationTarget item in result.Targets)
            {
                item.TargetPosition.State = item.NewState;
            }
        }
    }

    private void ProcessPassiveMutator(PassiveSelfMutator mutator)
    {
        SelfMutationResult result = mutator();
        if(result.StateChanged)
        {
            Cell.State = result.NewState;
        }
    }
}


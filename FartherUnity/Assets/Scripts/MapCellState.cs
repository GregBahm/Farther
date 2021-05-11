using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

public abstract class MapCellState
{
    private static readonly IEnumerable<Effector> NoEffectors = new Effector[0];
    private static readonly IEnumerable<CardDropEffector> NoCardDropEffectors = new CardDropEffector[0];

    public MapCell Cell { get; }

    public TerrainState Terrain { get; }

    public SiteType SiteType { get; }

    public Game Game { get { return Cell.Map.Game; } }

    protected delegate EffectorResult CardDropEffector(Card card);

    protected delegate EffectorResult Effector();

    // Evaluated when a card is dropped on the worldmap slot
    private readonly IEnumerable<CardDropEffector> dropMutators;

    // Evaluated when the state of a neighboring slot changes
    private readonly IEnumerable<Effector> onNeighborChangeEffectors;

    // Evaluated when a turn ends
    private readonly IEnumerable<Effector> onTurnEndEffectors;

    public MapCellState(MapCell cell,
        TerrainState terrain,
        SiteType siteType)
    {
        Cell = cell;
        Terrain = terrain;
        SiteType = siteType;
        dropMutators = GetDropMutators().ToArray();
        onNeighborChangeEffectors = GetOnNeighborChangeEffectors().ToArray();
        onTurnEndEffectors = GetOnTurnEndEffectors().ToArray();
    }

    public bool CanDropCardOnTile(Card card)
    {
        return dropMutators.Any(item => item(card).AnyEffect);
    }

    protected virtual IEnumerable<CardDropEffector> GetDropMutators()
    {
        return NoCardDropEffectors;
    }

    protected virtual IEnumerable<Effector> GetOnNeighborChangeEffectors()
    {
        return NoEffectors;
    }

    protected virtual IEnumerable<Effector> GetOnTurnEndEffectors()
    {
        return NoEffectors;
    }

    public EffectorResult GetFromDrop(Card card)
    {
        foreach (CardDropEffector item in dropMutators)
        {
            EffectorResult result = item(card);
            if (result.AnyEffect)
            {
                return result;
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
        foreach (Effector turnEndEffector in onTurnEndEffectors)
        {
            EventHandler action = (sender, e) => ProcessTurnEndEffector(turnEndEffector);
            Game.TurnEnd += action;
            turnEndListeners.Add(action);
        }

        foreach (MapCell neighbor in Cell.Neighbors)
        {
            foreach (Effector effector in onNeighborChangeEffectors)
            {
                EventHandler action = (sender, e) => effector().ApplyEffect(Game);
                neighbor.StateChanged += action;
                stateChangeListeners.Add(action);
            }
        }

        foreach(Effector effector in onNeighborChangeEffectors)
        {
            effector().ApplyEffect(Game);
        }
    }

    private void ProcessTurnEndEffector(Effector effector)
    {
        EffectorResult result = effector();

        foreach (MapCellState item in result.NewStates)
        {
            item.Cell.State = item;
        }
    }
}


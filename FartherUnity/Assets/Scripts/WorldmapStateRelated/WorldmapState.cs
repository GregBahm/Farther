using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

public abstract class WorldmapState
{
    protected WorldmapPosition MapPosition { get; private set; }

    public TerrainState Terrain { get; }

    public SiteType SiteType { get; }

    protected delegate StateChangeResult DropRecipe(Card card);

    protected delegate StateChangeResult PassiveRecipe();

    // Recipes that trigger when a card is dropped on the worldmap slot
    protected readonly IEnumerable<DropRecipe> dropRecipes;

    // Recipes that trigger when the state of a neighboring slot changes
    protected readonly IEnumerable<PassiveRecipe> onNeighborChangeRecipes;

    public WorldmapState(TerrainState terrain,
        SiteType siteType)
    {
        Terrain = terrain;
        SiteType = siteType;
        dropRecipes = GetDropRecipes().ToArray();
        onNeighborChangeRecipes = GetOnNeighborChangeRecipes().ToArray();
    }

    public bool CanDropCardOnTile(Card card)
    {
        return dropRecipes.Any(item => item(card).StateCanChange);
    }

    protected abstract IEnumerable<DropRecipe> GetDropRecipes();

    protected abstract IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes();

    public WorldmapState GetFromDrop(Card card)
    {
        foreach (var item in dropRecipes)
        {
            StateChangeResult result = item(card);
            if (result.StateCanChange)
            {
                return result.NewState;
            }
        }
        throw new InvalidOperationException("Can't GetFromDrop when no recipes can drop.");
    }

    private readonly List<EventHandler> stateChangeListeners = new List<EventHandler>();

    internal void OnRemovedFromMap()
    {
        if(MapPosition != null)
        {
            foreach (WorldmapPosition neighbor in MapPosition.Neighbors)
            {
                foreach (EventHandler listener in stateChangeListeners)
                {
                    neighbor.StateChanged -= listener;
                }
            }
        }
    }

    internal void OnAddedToMap(WorldmapPosition position)
    {
        MapPosition = position;

        foreach (WorldmapPosition neighbor in position.Neighbors)
        {
            foreach (PassiveRecipe passiveRecipe in onNeighborChangeRecipes)
            {
                EventHandler action = (sender, e) => ProcessPassiveRecipe(passiveRecipe);
                neighbor.StateChanged += action;
                stateChangeListeners.Add(action);
            }
        }

        foreach(PassiveRecipe recipe in onNeighborChangeRecipes)
        {
            ProcessPassiveRecipe(recipe);
        }
    }

    private void ProcessPassiveRecipe(PassiveRecipe recipe)
    {
        StateChangeResult result = recipe();
        if(result.StateCanChange)
        {
            MapPosition.State = result.NewState;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

public abstract class WorldmapState
{
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


    internal void OnRemovedFromMap()
    {
        // How do I remove these? Is this even correct? 
    }

    internal void OnAddedToMap(WorldmapSlot slot)
    {
        // TODO: Figure this out when I can iterate

        //foreach (var neighbor in slot.Neighbors)
        //{
        //    foreach (var neighborRecipe in onNeighborChangeRecipes)
        //    {
        //        neighbor.StateChanged += (sender, e) => neighborRecipe();
        //    }
        //}
    }
}


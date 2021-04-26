//using CardDropRecipes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
// Card recipes happen the moment a card is dropped, 
// and only effect the targeted cell
public class CardRecipeManager : IEnumerable<CardDropRecipe>
{
    private readonly IReadOnlyList<CardDropRecipe> recipes;

    public CardRecipeManager()
    {
        recipes = GetRecipes().ToList().AsReadOnly();
    }

    public IEnumerator<CardDropRecipe> GetEnumerator()
    {
        return recipes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<CardDropRecipe> GetRecipes()
    {
        yield return new EarthOnVoidToPlains();
        yield return new GreeneryOnPlainsToGrassland();
        yield return new GreeneryOnGrasslandToForest();
        yield return new FloodOnForestToSwamp();
        yield return new FloodOnPlainsToWetland();
        yield return new FloodOnVoidToSea();
        yield return new EarthOnLandToHills();
        yield return new EarthOnHillsToMountain();
    }
}
*/
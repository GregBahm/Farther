﻿using CardDropRecipes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        yield return new PlantsOnPlainsToGrassland();
        yield return new PlantsOnGrasslandToForest();
        //yield return new WaterOnForestToSwamp();
        //yield return new WaterOnPlainsToWetland();
        //yield return new WaterOnDesertToOasis();
        //yield return new ColdOnPlainsToTundra();
        //yield return new ColdOnGrasslandToColdGrassland();
        //yield return new ColdOnForestToColdForest();
        //yield return new HeatOnPlainsToDesert();
        //yield return new HeatOnGrasslandsToSavanah();
        //yield return new HeatOnForestToJungle();
        //yield return new WaterOnVoidToSeaOrCoast();
        //yield return new EarthOnLandToHills();
        //yield return new EarthOnHillsToMountain();
    }
}

// Passive recipes happen after any card is dropped,
// and can effect any cell
public class PassiveRecipeManager : IEnumerable<PassiveRecipe>
{
    private readonly IReadOnlyList<PassiveRecipe> recipes;

    public PassiveRecipeManager()
    {
        recipes = GetRecipes().ToList().AsReadOnly();
    }

    private IEnumerable<PassiveRecipe> GetRecipes()
    {
        yield return null;
        //yield return new TallestMountainRecipe();
        //yield return new RiverRecipe();
    }

    public IEnumerator<PassiveRecipe> GetEnumerator()
    {
        return recipes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public abstract class PassiveRecipe
{
    public abstract Dictionary<WorldmapCell, WorldmapState> GetModifiedCells(Worldmap worldMap);
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PassiveRecipes;

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
        yield return new TallestMountainRecipe();
        yield return new SeaToCoast();
        //yield return new CoastToLake(); TODO: Figure out coast to lake later
        yield return new OasisRecipe();
        yield return new RiverRecipe();
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

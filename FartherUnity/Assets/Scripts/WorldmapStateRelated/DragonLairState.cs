using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DragonLairState : WorldmapState
{
    public DragonLairState(WorldmapPosition position, TerrainState terrain, SiteType siteType) 
        : base(position, terrain, siteType)
    { }

    protected override IEnumerable<DropRecipe> GetDropRecipes()
    {
        // Drop wealth when hostile to give tribute
        // Drop warrior to delay or defeat hostile dragon
            // Need to set it up to where drops can yield cards
        // Drop warrior when allied to upgrade warrior
    }

    protected override IEnumerable<PassiveRecipe> GetOnNeighborChangeRecipes()
    {
        return new PassiveRecipe[0];
    }

    // TODO need to set up an update that drains a timer
    // And then when the timer falls, have the dragon start raiding
    // And then after raiding, grow in menace
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDropRecipes
{
    public class EarthOnVoidToPlains : CardDropRecipe
    {
        public override CardType Card => CardType.Earth;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Void;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();
            output.Terrain = MapTerrainType.Plains;
            return output.ToState();
        }
    }

    public class PlantsOnPlainsToGrassland : CardDropRecipe
    {
        public override CardType Card => CardType.Plant;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Plains;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Grassland;

            return output.ToState();
        }
    }

    public class PlantsOnGrasslandToForest : CardDropRecipe
    {
        public override CardType Card => CardType.Plant;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Grassland;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Forest;

            return output.ToState();
        }
    }
}

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

    public class WaterOnForestToSwamp : CardDropRecipe
    {
        public override CardType Card => CardType.Water;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Forest;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Swamp;

            return output.ToState();
        }
    }
    public class WaterOnPlainsToWetland : CardDropRecipe
    {
        public override CardType Card => CardType.Water;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Plains;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Wetland;

            return output.ToState();
        }
    }

    public class ColdOnPlainsToTundra : CardDropRecipe
    {
        public override CardType Card => CardType.Cold;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Plains
                && state.Neighbors.All(item => item?.Temperature <= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Tundra;
            output.Temperature = -1;

            return output.ToState();
        }
    }
    public class ColdOnGrasslandToColdGrassland : CardDropRecipe
    {
        public override CardType Card => CardType.Cold;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Grassland
                && state.Center.Temperature >= 0
                && state.Neighbors.All(item => item?.Temperature <= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Temperature = -1;

            return output.ToState();
        }
    }

    public class ColdOnForestToColdForest : CardDropRecipe
    {
        public override CardType Card => CardType.Cold;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Forest
                && state.Center.Temperature >= 0
                && state.Neighbors.All(item => item?.Temperature <= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Temperature = -1;

            return output.ToState();
        }
    }

    public class HeatOnPlainsToDesert : CardDropRecipe
    {
        public override CardType Card => CardType.Heat;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Plains
                && state.Neighbors.All(item => item?.Temperature >= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Temperature = 1;
            output.Terrain = MapTerrainType.Desert;

            return output.ToState();
        }
    }

    public class HeatOnGrasslandsToSavanah : CardDropRecipe
    {
        public override CardType Card => CardType.Heat;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Grassland
                && state.Neighbors.All(item => item?.Temperature >= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Temperature = 1;
            output.Terrain = MapTerrainType.Savannah;

            return output.ToState();
        }
    }

    public class HeatOnForestToJungle : CardDropRecipe
    {
        public override CardType Card => CardType.Heat;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Forest
                && state.Neighbors.All(item => item?.Temperature >= 0);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Temperature = 1;
            output.Terrain = MapTerrainType.Jungle;

            return output.ToState();
        }
    }
    public class WaterOnVoidToSea : CardDropRecipe
    {
        public override CardType Card => CardType.Water;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Void;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Sea;

            return output.ToState();
        }
    }
    public class EarthOnLandToHills : CardDropRecipe
    {
        private static readonly HashSet<MapTerrainType> hillTypes =
            new HashSet<MapTerrainType>() { 
                MapTerrainType.Desert,
                MapTerrainType.Forest,
                MapTerrainType.Grassland,
                MapTerrainType.Jungle,
                MapTerrainType.Plains,
                MapTerrainType.Savannah,
                MapTerrainType.Tundra,
            };

        public override CardType Card => CardType.Earth;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return !state.Center.Hill
                && hillTypes.Contains(state.Center.Terrain);
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Hill = true;

            return output.ToState();
        }
    }

    public class EarthOnHillsToMountain : CardDropRecipe
    {
        public override CardType Card => CardType.Earth;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Hill;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            output.Terrain = MapTerrainType.Mountain;

            return output.ToState();
        }
    }
}

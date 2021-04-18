using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;

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

            if(sourceState.Center.Temperature < 0)
                output.Terrain = MapTerrainType.Tundra;
            else if(sourceState.Center.Temperature > 0)
                output.Terrain = MapTerrainType.Desert;
            else
                output.Terrain = MapTerrainType.Plains;

            return output.ToState();
        }
    }

    public class GreeneryOnPlainsToGrassland : CardDropRecipe
    {
        public override CardType Card => CardType.Greenery;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Plains
                || state.Center.Terrain == MapTerrainType.Desert;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            if (sourceState.Center.Terrain > 0)
                output.Terrain = MapTerrainType.Savannah;
            else
                output.Terrain = MapTerrainType.Grassland;

            return output.ToState();
        }
    }

    public class GreeneryOnGrasslandToForest : CardDropRecipe
    {
        public override CardType Card => CardType.Greenery;

        public override bool CanModifyState(WorldmapStateWithNeighbors state)
        {
            return state.Center.Terrain == MapTerrainType.Grassland
                || state.Center.Terrain == MapTerrainType.Savannah;
        }

        public override WorldmapState ModifyState(WorldmapStateWithNeighbors sourceState)
        {
            WorldmapStateBuilder output = sourceState.Center.ToBuilder();

            if (sourceState.Center.Temperature > 0)
                output.Terrain = MapTerrainType.Jungle;
            else
                output.Terrain = MapTerrainType.Forest;

            return output.ToState();
        }
    }

    public class FloodOnForestToSwamp : CardDropRecipe
    {
        public override CardType Card => CardType.Flood;

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

    public class FloodOnPlainsToWetland : CardDropRecipe
    {
        public override CardType Card => CardType.Flood;

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

    public class FloodOnVoidToSea : CardDropRecipe
    {
        public override CardType Card => CardType.Flood;

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

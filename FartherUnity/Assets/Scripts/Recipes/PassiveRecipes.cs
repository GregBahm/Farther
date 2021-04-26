using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace PassiveRecipes
{
    public class TallestMountainRecipe : PassiveRecipe
    {
        public override Dictionary<WorldmapSlot, WorldmapState> GetModifiedCells(Worldmap worldMap)
        {
            Dictionary<WorldmapSlot, WorldmapState> ret = new Dictionary<WorldmapSlot, WorldmapState>();
            IEnumerable<WorldmapSlot> mountains = worldMap.Where(item => item.State.Terrain == MapTerrainType.Mountain);
            foreach(WorldmapSlot mountainCell in mountains)
            {
                if(mountainCell.Neighbors.All(item => item?.State.Terrain == MapTerrainType.Mountain))
                {
                    ret.Add(mountainCell, GetHighestPeak(mountainCell.State));
                }
            }
            return ret;
        }

        private WorldmapState GetHighestPeak(WorldmapState state)
        {
            var builder = state.ToBuilder();
            builder.Terrain = MapTerrainType.HighestPeak;
            return builder.ToState();
        }
    }

    public class RiverRecipe : PassiveRecipe
    {

        public override Dictionary<WorldmapSlot, WorldmapState> GetModifiedCells(Worldmap worldMap)
        {
            Dictionary<WorldmapSlot, WorldmapState> ret = new Dictionary<WorldmapSlot, WorldmapState>();
            IEnumerable<WorldmapSlot> plains = worldMap.Where(item => item.State.Terrain == MapTerrainType.Plains && !item.State.River);
            foreach (WorldmapSlot plainsCell in plains)
            {
                if(plainsCell.Neighbors.Count(item => IsWater(item)) == 1)
                {
                    ret.Add(plainsCell, GetWithRiver(plainsCell));
                }
            }
            return ret;
        }

        private WorldmapState GetWithRiver(WorldmapSlot cell)
        {
            WorldmapStateBuilder builder = cell.State.ToBuilder();
            builder.River = true;
            return builder.ToState();
        }

        private bool IsWater(WorldmapSlot item)
        {
            return item != null &&
                (IsWaterType(item.State.Terrain)
                || item.State.River);
        }
    }

    // If a sea touches land, it will become coast
    public class SeaToCoast : PassiveRecipe
    {
        public override Dictionary<WorldmapSlot, WorldmapState> GetModifiedCells(Worldmap worldMap)
        {
            Dictionary<WorldmapSlot, WorldmapState> ret = new Dictionary<WorldmapSlot, WorldmapState>();
            IEnumerable<WorldmapSlot> seas = worldMap.Where(item => item.State.Terrain == MapTerrainType.Sea);
            foreach (WorldmapSlot sea in seas)
            {
                if(sea.Neighbors.Any(item => IsLand(item)))
                {
                    ret.Add(sea, GetAsCoast(sea.State));
                }
            }
            return ret;
        }

        private WorldmapState GetAsCoast(WorldmapState state)
        {
            WorldmapStateBuilder builder = state.ToBuilder();
            builder.Terrain = MapTerrainType.Coast;
            return builder.ToState();
        }

        private bool IsLand(WorldmapSlot item)
        {
            return item != null
                && item.State.Terrain != MapTerrainType.Void
                && !IsWaterType(item.State.Terrain);
        }
    }


    // If a lake is surrounded by desert, it will become an oasis
    public class OasisRecipe : PassiveRecipe
    {

        public override Dictionary<WorldmapSlot, WorldmapState> GetModifiedCells(Worldmap worldMap)
        {
            Dictionary<WorldmapSlot, WorldmapState> ret = new Dictionary<WorldmapSlot, WorldmapState>();
            IEnumerable<WorldmapSlot> desertLakes = worldMap.Where(item => item.State.Terrain == MapTerrainType.Lake 
            && item.Neighbors.All(neighbor => neighbor?.State.Terrain == MapTerrainType.Desert));
            foreach (WorldmapSlot desertLake in desertLakes)
            {
                    ret.Add(desertLake, GetAsOasis(desertLake.State));
            }
            return ret;
        }

        private WorldmapState GetAsOasis(WorldmapState state)
        {
            WorldmapStateBuilder builder = state.ToBuilder();
            builder.Terrain = MapTerrainType.Oasis;
            return builder.ToState();
        }
    }
}
*/
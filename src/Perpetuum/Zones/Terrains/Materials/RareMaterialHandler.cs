using Perpetuum.Data;
using Perpetuum.Items;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Zones.Terrains.Materials
{
    public class RareMaterialHandler
    {
        private readonly ILookup<int, RareMaterialInfo> _rareMaterialInfos;

        public RareMaterialHandler()
        {
            _rareMaterialInfos = Database.CreateLookupCache<int, RareMaterialInfo, DataContext.Entities.Rarematerial>(
                x => x.Definition,
                RareMaterialInfo.CreateFromDbDataRecord
            );
        }

        public List<ItemInfo> GenerateRareMaterials(int definition)
        {
            var result = new List<ItemInfo>();
            foreach (var rareMaterialInfo in _rareMaterialInfos.GetOrEmpty(definition))
            {
                var random = FastRandom.NextDouble();
                if (random < rareMaterialInfo.chance)
                    result.Add(rareMaterialInfo.itemInfo);
            }
            return result;
        }

        private class RareMaterialInfo
        {
            public readonly ItemInfo itemInfo;
            public readonly double chance;

            private RareMaterialInfo(ItemInfo itemInfo, double chance)
            {
                this.itemInfo = itemInfo;
                this.chance = chance;
            }

            public static RareMaterialInfo CreateFromDbDataRecord(DataContext.Entities.Rarematerial entity)
            {
                var definition = entity.Raredefinition;
                var quantity = entity.Quantity;
                var chance = entity.Chance;

                return new RareMaterialInfo(new ItemInfo(definition, quantity), chance);
            }
        }
    }
}
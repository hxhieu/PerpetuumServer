using Perpetuum.Builders;
using Perpetuum.Services.Looting;

namespace Perpetuum.Zones.Artifacts
{
    public interface IArtifactLoot
    {
        double Chance { get; }
        IBuilder<LootItem> GetLootItemBuilder();
    }

    /// <summary>
    /// Describes one loot item can be found in a discovered artifact
    /// </summary>
    public class ArtifactLoot : IArtifactLoot
    {
        private int Definition { get; set; }
        private IntRange Quantity { get; set; }
        public double Chance { get; private set; }

        public IBuilder<LootItem> GetLootItemBuilder()
        {
            return LootItemBuilder.Create(Definition)
                .SetQuantity(FastRandom.NextInt(Quantity))
                .SetRepackaged(Packed);
        }

        private bool Packed { get; set; }

        public ArtifactLoot(DataContext.Entities.Artifactloot entity)
        {
            Definition = entity.Definition;
            Quantity = new IntRange(entity.Minquantity, entity.Maxquantity);
            Chance = entity.Chance;
            Packed = entity.Packed;
        }
    }
}
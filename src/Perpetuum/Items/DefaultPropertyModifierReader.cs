using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.ExportedTypes;
using System;
using System.Linq;

namespace Perpetuum.Items
{
    public class DefaultPropertyModifierReader(IDbRepository<Aggregatevalue> aggValueRepo)
    {
        private ILookup<int, ItemPropertyModifier> modifiers;

        public void Init()
        {
            modifiers = aggValueRepo.GetMany(cacheTime: TimeSpan.FromHours(1))  // Can cache for a very long time?
                .ToLookup(e => e.Definition, e => ItemPropertyModifier.Create((AggregateField)e.Field, e.Value));
        }

        public ItemPropertyModifier[] GetByDefinition(int definition)
        {
            return modifiers.GetOrEmpty(definition);
        }
    }
}

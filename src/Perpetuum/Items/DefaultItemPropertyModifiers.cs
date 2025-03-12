using Perpetuum.Data;
using Perpetuum.DataContext.Entities;
using Perpetuum.ExportedTypes;
using System.Linq;

namespace Perpetuum.Items
{
    public static class DefaultItemPropertyModifiers
    {
        private static readonly ILookup<int,ItemPropertyModifier> _defaultProperties;

        static DefaultItemPropertyModifiers()
        {
            _defaultProperties = Database.CreateLookupCache<int, ItemPropertyModifier, Aggregatevalue>(
                x => x.Definition,
                x =>
                {
                    var field = (AggregateField)x.Field;
                    var value = x.Value;
                    return ItemPropertyModifier.Create(field, value);
                }
            );
        }

        public static ItemPropertyModifier[] GetPropertyModifiers(int definition)
        {
            return _defaultProperties.GetOrEmpty(definition);
        }
    }
}
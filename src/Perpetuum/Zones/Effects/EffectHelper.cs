using Perpetuum.Data;
using Perpetuum.ExportedTypes;
using Perpetuum.Items;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Zones.Effects
{

    /// <summary>
    /// Effect helper class
    /// </summary>
    public static class EffectHelper
    {
        public static readonly IDictionary<EffectCategory, int> EffectCategoryLevels = Database.CreateCache<EffectCategory, int, DataContext.Entities.Effectcategory>(
            x => (EffectCategory)x.Flag,
            x => x.Maxlevel,
            null,
            key => (EffectCategory)(1L << (int)key)
        );

        private static readonly IDictionary<EffectType, EffectInfo> _effectInfos = Database.CreateCache<EffectType, EffectInfo, DataContext.Entities.Effect>(
            x => (EffectType)x.Id,
            x => new EffectInfo(x)
        );

        private static readonly ILookup<EffectType, ItemPropertyModifier> _effectDefaultModifiers;

        static EffectHelper()
        {
            _effectDefaultModifiers = Database.CreateLookupCache<EffectType, ItemPropertyModifier, DataContext.Entities.Effectdefaultmodifier>(
                x => (EffectType)x.Effectid,
                x =>
                {
                    var field = (AggregateField)x.Field;
                    var value = x.Value;
                    return ItemPropertyModifier.Create(field, value);
                }
            );
        }

        public static EffectInfo GetEffectInfo(EffectType effectType)
        {
            return _effectInfos[effectType];
        }

        public static IEnumerable<ItemPropertyModifier> GetEffectDefaultModifiers(EffectType effectType)
        {
            return _effectDefaultModifiers[effectType];
        }

        public static Dictionary<string, object> GetEffectInfosDictionary()
        {
            return _effectInfos.Values.ToDictionary("e", ei => ei.ToDictionary());
        }

        public static Dictionary<string, object> GetEffectDefaultModifiersDictionary()
        {
            var counter = 0;
            var result = new Dictionary<string, object>();
            foreach (var effectDefaultModifier in _effectDefaultModifiers)
            {
                var oneEntry = new Dictionary<string, object> {{k.effectType, (int) effectDefaultModifier.Key}};

                var effectsDict = new Dictionary<string, object>();

                foreach (var effect in effectDefaultModifier)
                {
                    effect.AddToDictionary(effectsDict);
                }

                oneEntry.Add(k.effect, effectsDict);
                result.Add("e" + counter++, oneEntry);
            }

            return result;
        }
    }
}
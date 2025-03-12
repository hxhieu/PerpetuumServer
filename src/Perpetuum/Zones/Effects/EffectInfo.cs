using Perpetuum.ExportedTypes;
using System.Collections.Generic;

namespace Perpetuum.Zones.Effects
{
    public class EffectInfo
    {
        public readonly EffectType type;
        public readonly EffectCategory category;
        public readonly int duration;
        private readonly string _name;
        private readonly string _description;
        private readonly bool _isPositive;
        public readonly bool isAura;
        public readonly int auraRadius;
        private readonly int _displayFlags;

        public EffectInfo(DataContext.Entities.Effect entity)
        {
            type = (EffectType)entity.Id;
            category = (EffectCategory)entity.Effectcategory;
            _name = entity.Name;
            duration = entity.Duration;
            _description = entity.Description;
            _isPositive = entity.Ispositive;
            isAura = entity.Isaura;
            auraRadius = entity.Auraradius;
            _displayFlags = entity.Display;
        }

        public bool Display
        {
            get
            {
                return _displayFlags > 0;
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
                       {
                               {k.type, (int) type},
                               {k.name, _name},
                               {k.category, (long) category},
                               {k.duration, duration},
                               {k.description, _description},
                               {k.isPositive, _isPositive},
                               {k.isAura, isAura},
                               {k.auraRadius, auraRadius},
                               {k.display, _displayFlags}
                       };
        }
    }
}
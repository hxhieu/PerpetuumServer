using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;
using Perpetuum.ExportedTypes;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Modules
{
    public class ModulePropertyModifiersReader(IEntityDefaultReader entityDefaultReader, IDbRepository<Modulepropertymodifier> modulePropModRepo)
    {
        private Dictionary<int, ILookup<AggregateField, AggregateField>> _modifiers;

        public void Init()
        {
            var records = modulePropModRepo.GetMany()
                .Select(e => new
                {
                    categoryFlags = (CategoryFlags)e.Categoryflags,
                    baseField = (AggregateField)e.Basefield,
                    modifierField = (AggregateField)e.Modifierfield
                })
                .Where(r => r.modifierField != AggregateField.undefined)
                .ToLookup(r => r.categoryFlags);


            IEnumerable<EntityDefault> modules = entityDefaultReader.GetAll().GetByCategoryFlags(CategoryFlags.cf_robot_equipment);
            _modifiers = new Dictionary<int, ILookup<AggregateField, AggregateField>>();

            foreach (EntityDefault ed in modules)
            {
                List<KeyValuePair<AggregateField, AggregateField>> p = new List<KeyValuePair<AggregateField, AggregateField>>();

                foreach (CategoryFlags cf in ed.CategoryFlags.GetCategoryFlagsTree())
                {
                    foreach (var record in records.GetOrEmpty(cf))
                    {
                        p.Add(new KeyValuePair<AggregateField, AggregateField>(record.baseField, record.modifierField));
                    }

                    if (cf == CategoryFlags.cf_robot_equipment)
                    {
                        break;
                    }
                }

                _modifiers[ed.Definition] = p.ToLookup(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        public ILookup<AggregateField, AggregateField> GetModifiers(Module module)
        {
            return _modifiers.GetOrDefault(module.Definition);
        }
    }
}
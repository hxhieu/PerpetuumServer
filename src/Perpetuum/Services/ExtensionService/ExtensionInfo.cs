using System.Collections.Generic;
using System.Data;
using Perpetuum.Data;
using Perpetuum.ExportedTypes;

namespace Perpetuum.Services.ExtensionService
{
    public class ExtensionInfo
    {
        public int id;
        public string name;
        public int category;
        public int rank;
        public string learningAttributePrimary;
        public string learningAttributeSecondary;
        public double bonus;
        public int price;
        private string _description;
        public AggregateField aggregateField;
        public bool hidden;
        public int? freezeLimit;

        public Extension[] RequiredExtensions { get; set; }

        public ExtensionInfo(DataContext.Entities.Extension entity)
        {
            _description = entity.Description;
        }

        public override string ToString()
        {
            return $"name:{name} id:{id}";
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
                {
                    {k.extensionID, id},
                    {k.name, name},
                    {k.category, category},
                    {k.rank, rank},
                    {k.price, price},
                    {k.bonus, bonus},
                    {k.learningAttributePrimary, learningAttributePrimary},
                    {k.learningAttributeSecondary, learningAttributeSecondary},
                    {k.description, _description},
                    {k.hidden, hidden},
                    {k.freezeLimit, freezeLimit},
                };
        }
    }
}
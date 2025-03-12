using Perpetuum.Data;
using Perpetuum.DataContext.Entities;
using Perpetuum.ExportedTypes;
using System.Collections.Generic;

namespace Perpetuum
{
    public static class AggregateFieldExtensions
    {
        private static readonly IDictionary<AggregateField, AggregateFormula> _formulas;

        static AggregateFieldExtensions()
        {
            _formulas = Database.CreateCache<AggregateField, AggregateFormula, Aggregatefield>(
                x => (AggregateField)x.Id,
                x => (AggregateFormula)x.Formula
            );
        }

        public static AggregateFormula GetFormula(this AggregateField field)
        {
            AggregateFormula formula;
            if (!_formulas.TryGetValue(field, out formula))
                formula = AggregateFormula.Add;

            return formula;
        }

        public static bool IsPublic(this AggregateField field)
        {
            switch (field)
            {
                case AggregateField.armor_max:
                case AggregateField.armor_current:
                case AggregateField.speed_max:
                case AggregateField.speed_current:
                case AggregateField.core_max:
                case AggregateField.core_current:
                    return true;
            }

            return false;
        }

        public static double GetDefaultValue(this AggregateField field)
        {
            var formula = field.GetFormula();
            return (formula == AggregateFormula.Modifier || formula == AggregateFormula.Inverse) ? 1.0 : 0.0;
        }
    }
}

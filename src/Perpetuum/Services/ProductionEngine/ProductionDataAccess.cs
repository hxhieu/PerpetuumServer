using Perpetuum.Data;
using Perpetuum.EntityFramework;
using Perpetuum.ExportedTypes;
using Perpetuum.Log;
using Perpetuum.Services.ProductionEngine.CalibrationPrograms;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Services.ProductionEngine
{
    public class ProductionDataAccess : IProductionDataAccess
    {
        private readonly IEntityDefaultReader _entityDefaultReader;
        private IDictionary<int, int> _prototypes;
        private IDictionary<int, ItemResearchLevel> _researchlevels;
        private ILookup<int, ProductionComponent> _productionComponents;
        private IDictionary<CategoryFlags, double> _productionDurations;
        private IDictionary<int, CalibrationDefault> _calibrationDefaults;
        private IDictionary<CategoryFlags, ProductionDecalibration> _productionDecalibrations;
        private IProductionCostReader _productionCostReader;

        public ProductionDataAccess(IEntityDefaultReader entityDefaultReader, IProductionCostReader costReader)
        {
            _entityDefaultReader = entityDefaultReader;
            _productionCostReader = costReader;
        }

        public void Init()
        {
            _prototypes = Database.CreateCache<int, int, DataContext.Entities.Prototype>(
                x => x.Definition,
                x => x.Prototype1
            );
            _researchlevels = Database.CreateCache<int, ItemResearchLevel, DataContext.Entities.Itemresearchlevel>(
                x => x.Definition,
                x =>
                {
                    var level = new ItemResearchLevel
                    {
                        definition = x.Definition,
                        researchLevel = x.Researchlevel,
                        calibrationProgramDefinition = x.Calibrationprogram
                    };
                    return level;
                },
                ItemResearchLevelFilter
            );

            _productionComponents = Database.CreateLookupCache<int, ProductionComponent, DataContext.Entities.Component>(
                x => x.Definition,
                x =>
                {
                    var ed = _entityDefaultReader.Get(x.Componentdefinition);
                    var amount = x.Componentamount;
                    return new ProductionComponent(ed, amount);
                },
                x => _entityDefaultReader.Exists(x.Definition)
            );

            _productionDurations = Database.CreateCache<CategoryFlags, double, DataContext.Entities.Productionduration>(
                x => (CategoryFlags)x.Category,
                x => x.Durationmodifier
            );
            _calibrationDefaults = Database.CreateCache<int, CalibrationDefault, DataContext.Entities.Calibrationdefault>(
                x => x.Definition,
                x => new CalibrationDefault(x)
            );
            _productionDecalibrations = Database.CreateCache<CategoryFlags, ProductionDecalibration, DataContext.Entities.Productiondecalibration>(
                x => (CategoryFlags)x.Categoryflag,
                x =>
                {
                    var distorsionMin = x.Distorsionmin;
                    var distorsionMax = x.Distorsionmax;
                    var decrease = x.Decrease ?? 0;
                    return new ProductionDecalibration(distorsionMin, distorsionMax, decrease);
                }
            );

            _researchlevels = Database.CreateCache<int, ItemResearchLevel, DataContext.Entities.Itemresearchlevel>(
                x => x.Definition,
                x =>
                {
                    var level = new ItemResearchLevel
                    {
                        definition = x.Definition,
                        researchLevel = x.Researchlevel,
                        calibrationProgramDefinition = x.Calibrationprogram
                    };
                    return level;
                },
                ItemResearchLevelFilter
            );

            ProductionCost = new Dictionary<int, double>();
            foreach (var ed in EntityDefault.All)
            {
                ProductionCost.Add(ed.Definition, _productionCostReader.GetProductionCostModByED(ed));
            }
        }

        public bool ItemResearchLevelFilter(DataContext.Entities.Itemresearchlevel record)
        {
            var definition = record.Definition;

            if (!_entityDefaultReader.Exists(definition) || !record.Enabled)
            {
                return false;
            }

            var calibrationPrg = record.Calibrationprogram;
            if (calibrationPrg == null)
                return true;

            var cprgED = _entityDefaultReader.Get((int)calibrationPrg);
            if (cprgED.CategoryFlags.IsCategory(CategoryFlags.cf_calibration_programs))
                return true;

            Logger.Error("illegal calibration program was defined for definition:" + definition + " calibration program def:" + cprgED.Name + " " + cprgED.Definition);
            return false;
        }

        public IDictionary<int, int> Prototypes => _prototypes;
        public IDictionary<int, ItemResearchLevel> ResearchLevels => _researchlevels;
        public ILookup<int, ProductionComponent> ProductionComponents => _productionComponents;
        public IDictionary<CategoryFlags, double> ProductionDurations => _productionDurations;
        public IDictionary<int, CalibrationDefault> CalibrationDefaults => _calibrationDefaults;
        public IDictionary<int, double> ProductionCost { get; private set; }

        public ProductionDecalibration GetDecalibration(int targetDefinition)
        {
            if (!_entityDefaultReader.TryGet(targetDefinition, out EntityDefault ed))
            {
                Logger.Error("consistency error! definition was not found for production line. definition:" + targetDefinition);
                return ProductionDecalibration.Default;
            }

            return GetDecalibration(ed);
        }

        public ProductionDecalibration GetDecalibration(EntityDefault target)
        {
            foreach (var flagInTree in target.CategoryFlags.GetCategoryFlagsTree())
            {
                if (_productionDecalibrations.TryGetValue(flagInTree, out ProductionDecalibration productionDecalibration))
                    return productionDecalibration;
            }

            return ProductionDecalibration.Default;
        }
    }
}
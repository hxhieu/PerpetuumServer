namespace Perpetuum.Services.ProductionEngine.CalibrationPrograms
{

    /// <summary>
    /// Default calibration data for dynamic calibration programs
    /// 
    /// </summary>
    public class DynamicCalibrationTemplate
    {
        public readonly double MaterialEfficiency;
        public readonly double TimeEfficiency;
        public readonly int TargetDefinition;

        public DynamicCalibrationTemplate(DataContext.Entities.Dynamiccalibrationtemplate entity)
        {
            MaterialEfficiency = entity.Materialefficiency;
            TimeEfficiency = entity.Timeefficiency;
            TargetDefinition = entity.Targetdefinition;
        }
    }
}

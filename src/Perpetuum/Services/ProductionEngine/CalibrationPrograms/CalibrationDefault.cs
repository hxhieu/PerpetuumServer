namespace Perpetuum.Services.ProductionEngine.CalibrationPrograms
{

    /// <summary>
    /// Default calibration of a CPRG
    /// 
    /// </summary>
    public class CalibrationDefault
    {
        public double materialEfficiency;
        public double timeEfficiency;

        public CalibrationDefault(DataContext.Entities.Calibrationdefault entity)
        {
            materialEfficiency = entity.Materialefficiency;
            timeEfficiency = entity.Timeefficiency;
        }
    }
}

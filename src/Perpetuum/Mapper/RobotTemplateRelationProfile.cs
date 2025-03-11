using Perpetuum.DataContext.Entities;
using Perpetuum.Items.Templates;

namespace Perpetuum.Mapper
{
    internal class RobotTemplateRelationProfile : AutoMapper.Profile
    {
        public RobotTemplateRelationProfile()
        {
            //EntityDefault = _entityDefaultReader.Get(record.GetValue<int>("definition")),
            //Template = _robotTemplateReader.Get(record.GetValue<int>("templateid")),
            //RaceID = record.GetValue<int>("raceid"),
            //missionLevel = record.GetValue<int?>("missionlevel"),
            //missionLevelOverride = record.GetValue<int?>("missionleveloverride")

            // Done by conventions
            CreateMap<Robottemplaterelation, RobotTemplateRelation>();
        }
    }
}

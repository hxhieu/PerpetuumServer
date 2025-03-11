using Perpetuum.Zones;
using System.Drawing;

namespace Perpetuum.Mapper
{
    internal class ZoneConfigurationProfile : AutoMapper.Profile
    {
        public ZoneConfigurationProfile()
        {
            //int x = record.GetValue<int>("x");
            //int y = record.GetValue<int>("y");
            //int w = record.GetValue<int>("width");
            //int h = record.GetValue<int>("height");
            //int id = record.GetValue<int>("id");

            //ZoneConfiguration config = new ZoneConfiguration
            //{
            //    Id = id,
            //    WorldPosition = new Point(x, y),
            //    Size = new Size(w, h),
            //    Name = record.GetValue<string>("name"),
            //    Fertility = record.GetValue<int>("fertility"),
            //    PluginName = record.GetValue<string>("zoneplugin"),
            //    //ListenerAddress = "127.0.0.1",
            //    ListenerPort = port++,
            //    NpcSpawnId = record.GetValue<int?>("spawnid") ?? 0,
            //    Protected = record.GetValue<bool>("protected"),
            //    Terraformable = record.GetValue<bool>("terraformable"),
            //    RaceId = record.GetValue<int>("raceid"),
            //    plantRuleSetId = record.GetValue<int>("plantruleset"),
            //    Type = (ZoneType)record.GetValue<int>("zonetype"),
            //    SparkCost = record.GetValue<int>("sparkcost"),
            //    MaxPlayers = 10000,
            //    MaxDockingBase = record.GetValue<int>("maxdockingbase"),
            //    PlantAltitudeScale = record.GetValue<double>("plantaltitudescale"),
            //    Note = record.GetValue<string>("note"),

            //    TimeLimitMinutes = record.GetValue<int?>("timeLimitMinutes"),
            //    PBSTechLimit = record.GetValue<int?>("pbsTechLimit") ?? 0,
            //    PlantsGrowthTimerOverrideMin = record.GetValue<int?>("PlantsGrowthTimerOverrideMin"),
            //};

            CreateMap<DataContext.Entities.Zone, ZoneConfiguration>()
                .ForMember(dest => dest.WorldPosition, opt => opt.MapFrom(src => new Point(src.X, src.Y)))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => new Size(src.Width, src.Height)))
                .ForMember(dest => dest.PluginName, opt => opt.MapFrom(src => src.Zoneplugin))
                .ForMember(dest => dest.NpcSpawnId, opt => opt.MapFrom(src => src.Spawnid))
                .ForMember(dest => dest.plantRuleSetId, opt => opt.MapFrom(src => src.Plantruleset))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (ZoneType)src.Zonetype))
                .ForMember(dest => dest.MaxPlayers, opt => opt.MapFrom(src => 10))
                .ForMember(dest => dest.PBSTechLimit, opt => opt.MapFrom(src => src.PbsTechLimit ?? 0))
            ;
        }
    }
}

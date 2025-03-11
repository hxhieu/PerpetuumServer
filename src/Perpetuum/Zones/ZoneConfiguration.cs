using AutoMapper;
using Perpetuum.DataContext;
using Perpetuum.EntityFramework;
using Perpetuum.ExportedTypes;
using Perpetuum.Zones.Terrains.Materials.Plants;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Perpetuum.Zones
{
    public interface IZoneConfigurationReader
    {
        IEnumerable<ZoneConfiguration> GetAll();
    }

    public class ZoneConfigurationReader(
        GlobalConfiguration globalConfiguration,
        PlantRuleLoader plantRuleLoader,
        IMapper mapper,
        IDbRepository<DataContext.Entities.Zone> zoneRepo
    ) : IZoneConfigurationReader
    {
        public IEnumerable<ZoneConfiguration> GetAll()
        {
            int port = globalConfiguration.ListenerPort + 1;

            var result = zoneRepo.GetMany(x => x.Enabled).Select(e =>
            {
                var config = mapper.Map<ZoneConfiguration>(e);
                config.ListenerPort = port++;
                config.MaxPlayers = 10000;
                config.PlantRules = plantRuleLoader.LoadPlantRulesWithOverrides(config.plantRuleSetId);

                return config;
            });

            return result;
        }
    }

    public sealed class ZoneConfiguration
    {
        public static readonly ZoneConfiguration None = new ZoneConfiguration { Id = -1 };
        private const int WATER_LEVEL = 55;

        private static readonly Dictionary<int, string> _raceIDToTeleport = new Dictionary<int, string>
        {
            {1, DefinitionNames.PUBLIC_TELEPORT_COLUMN_PELISTAL},
            {2, DefinitionNames.PUBLIC_TELEPORT_COLUMN_NUIMQOL},
            {3, DefinitionNames.PUBLIC_TELEPORT_COLUMN_THELODICA}
        };

        public int plantRuleSetId;

        public int Id { get; set; }
        public Size Size { get; set; }
        public Point WorldPosition { get; set; }
        public string PluginName { get; set; }
        public int Fertility { get; set; }
        public bool Terraformable { get; set; }
        public bool Protected { get; set; }
        public int NpcSpawnId { get; set; }
        public int MaxPlayers { get; set; }
        public int SparkCost { get; set; }
        public int MaxDockingBase { get; set; }
        public double PlantAltitudeScale { private get; set; }
        public int RaceId { get; set; }
        public string Note { get; set; }

        public string ListenerAddress { get; set; } = "127.0.0.1";
        public int ListenerPort { get; set; }

        public int? TimeLimitMinutes { get; set; }
        public int PBSTechLimit { get; set; }

        public int? PlantsGrowthTimerOverrideMin { get; set; }

        public ZoneType Type { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.zone, Id},
                {k.position, WorldPosition},
                {k.name, Name},
                {k.fertility, Fertility},
                {k.zonePlugin, PluginName},
                {k.zoneIP,ListenerAddress},
                {k.zonePort,ListenerPort},
                {k.instance, false},
                {k.spawnID, NpcSpawnId},
                {k.plantRuleSetID, plantRuleSetId},
                {k.isProtected, Protected},
                {k.raceID, RaceId},
                {k.width, Size.Width},
                {k.height, Size.Height},
                {k.terraformable, Terraformable},
                {k.waterLevel, WaterLevel},
                {k.type, (int) Type},
                {k.sparkCost, SparkCost},
                {k.maxDockingBase, MaxDockingBase},
                {k.note, Note }
            };
        }

        public override string ToString()
        {
            return $"Id:{Id} Name:{Name} Protected:{Protected} Terraformable:{Terraformable}";
        }

        public EntityDefault TeleportColumn
        {
            get
            {
                string name = _raceIDToTeleport.GetOrDefault(RaceId, DefinitionNames.PUBLIC_TELEPORT_COLUMN_PELISTAL);
                return EntityDefault.GetByName(name);
            }
        }

        public string Name { get; set; }

        public ZoneStorage GetStorage()
        {
            return ZoneStorage.Get(this);
        }

        public List<PlantRule> PlantRules { get; set; }

        public static int WaterLevel => WATER_LEVEL;

        public bool IsAlpha => Protected;

        public bool IsBeta => !Protected && !Terraformable;

        public bool IsGamma => Terraformable;
    }
}



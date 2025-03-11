using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;

namespace Perpetuum.Mapper
{
    internal class DefinitionConfigProfile: AutoMapper.Profile
    {
        public DefinitionConfigProfile()
        {
            //definition = record.GetValue<int>("definition");
            //targetDefinition = record.GetValue<int?>("targetdefinition");
            //npcPresenceId = record.GetValue<int?>("npcpresenceid");
            //item_work_range = record.GetValue<double?>("item_work_range");
            //explosion_radius = record.GetValue<double?>("explosion_radius");
            //cycle_time = record.GetValue<int?>("cycle_time");
            //damage_chemical = record.GetValue<double?>("damage_chemical");
            //damage_explosive = record.GetValue<double?>("damage_explosive");
            //damage_kinetic = record.GetValue<double?>("damage_kinetic");
            //damage_thermal = record.GetValue<double?>("damage_thermal");
            //damage_toxic = record.GetValue<double?>("damage_toxic");
            //lifeTime = record.GetValue<int?>("lifetime");
            //activationTime = record.GetValue<int?>("activationtime");
            //waves = record.GetValue<int?>("waves");
            //missionRelated = record.GetValue<bool?>("missionrelated");
            //constructionRadius = record.GetValue<int?>("constructionradius");
            //_actionDelay = record.GetValue<int?>("action_delay");
            //deploy_radius = record.GetValue<int?>("deploy_radius");
            //transmitradius = record.GetValue<int?>("transmitradius");
            //constructionlevelmax = record.GetValue<int?>("constructionlevelmax");
            //blockingradius = record.GetValue<int?>("blockingradius");
            //chargeAmount = record.GetValue<int?>("chargeAmount");
            //inConnections = record.GetValue<int?>("inconnections");
            //outConnections = record.GetValue<int?>("outconnections");
            //coreTransferred = record.GetValue<double?>("coretransferred");
            //transferEfficiency = record.GetValue<double?>("transferefficiency");
            //productionUpgradeAmount = record.GetValue<int?>("productionupgradeamount");
            //productionLevel = record.GetValue<int?>("productionlevel");
            //_coreConsumption = record.GetValue<double?>("coreconsumption");
            //effectId = record.GetValue<int?>("effectid");
            //_coreCalories = record.GetValue<double?>("corecalories") ?? 0;
            //coreKickStartThreshold = record.GetValue<double?>("corekickstartthreshold");
            //reinforceCounterMax = record.GetValue<int?>("reinforcecountermax");
            //bandwidthUsage = record.GetValue<int?>("bandwidthusage");
            //bandwidthCapacity = record.GetValue<int?>("bandwidthcapacity");
            //emitRadius = record.GetValue<int?>("emitradius");
            //typeExclusiveRange = record.GetValue<int?>("typeexclusiverange");
            //network_node_range = record.GetValue<int?>("network_node_range");
            //_hitSize = record.GetValue<double?>("hitsize") ?? 1.41;

            //var tint = record.GetValue<string>("tint");

            //if (!string.IsNullOrEmpty(tint))
            //{
            //    _tint = ColorTranslator.FromHtml(tint);
            //}

            // Done by convention
            CreateMap<Definitionconfig, DefinitionConfig>();

        }
    }
}

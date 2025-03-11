using Perpetuum.DataContext.Entities;
using Perpetuum.Log;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Perpetuum.EntityFramework
{
    public class DefinitionConfig
    {
        public static readonly DefinitionConfig None = new DefinitionConfig();

        public readonly int definition;
        public int? targetDefinition;
        public int? npcPresenceId;
        public double? item_work_range;
        public double? explosion_radius;
        public int? cycle_time;
        public double? damage_chemical;
        public double? damage_explosive;
        public double? damage_kinetic;
        public double? damage_thermal;
        public double? damage_toxic;
        public int? lifeTime;
        public int? activationTime;
        public int? waves;
        public bool? missionRelated;
        public int? constructionRadius;
        private double? _actionDelay;
        public int? deploy_radius;
        public int? transmitradius;
        public int? constructionlevelmax;
        public int? blockingradius;
        public int? chargeAmount;
        public int? inConnections;
        public int? outConnections;
        public double? coreTransferred;
        public double? transferEfficiency;
        public int? productionUpgradeAmount;
        public int? productionLevel;
        private double? _coreConsumption;
        public int? effectId;
        public double? coreKickStartThreshold;
        public int? reinforceCounterMax;
        public int? bandwidthUsage;
        public int? bandwidthCapacity;
        public int? emitRadius;
        public int? typeExclusiveRange;
        public int? network_node_range;

        private readonly double _hitSize;

        private readonly Color _tint = Color.White;

        private readonly double? _coreCalories;

        public double CoreCalories
        {
            get { return _coreCalories ?? 0; }
        }

        private DefinitionConfig()
        {
        }

        public double CoreConsumption
        {
            get
            {
                double v;
                if (_coreConsumption.TryGetValue(out v))
                    return v;

                Logger.Warning($"no coreconsumption found for definition: {definition}");
                v = 0;
                return v;
            }
        }

        public EntityDefault TargetEntityDefault
        {
            get { return EntityDefault.GetOrThrow(targetDefinition ?? 0); }
        }

        public int ConstructionRadius
        {
            get { return (int) constructionRadius.ThrowIfNull(ErrorCodes.ServerError); }
        }

        public Color Tint
        {
            get { return _tint; }
        }

        public TimeSpan ActionDelay
        {
            get { return TimeSpan.FromMilliseconds((double) _actionDelay.ThrowIfNull(ErrorCodes.ServerError)); }
        }

        public double HitSize => _hitSize;

        public DefinitionConfig(Definitionconfig entity)
        {
            definition = entity.Definition;
            _actionDelay = entity.ActionDelay;
            _coreConsumption = entity.Coreconsumption;
            _coreCalories = entity.Corecalories;
            _hitSize = entity.Hitsize ?? 1.41;

            var tint = entity.Tint;

            if (!string.IsNullOrEmpty(tint))
            {
                _tint = ColorTranslator.FromHtml(tint);
            }
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.targetDefinition, targetDefinition},
                {k.item_work_range, item_work_range},
                {k.explosion_radius, explosion_radius},
                {k.cycle_time, cycle_time},
                {k.damage_chemical, damage_chemical},
                {k.damage_explosive, damage_explosive},
                {k.damage_kinetic, damage_kinetic},
                {k.damage_thermal, damage_thermal},
                {k.damage_toxic, damage_toxic},
                {k.lifeTime, lifeTime},
                {k.activationTime, activationTime},
                {k.waves, waves},
                {k.constructionRadius, constructionRadius},
                {k.action_delay, _actionDelay},
                {k.deploy_radius, deploy_radius},
                {k.transmitRadius, transmitradius},
                {k.constructionLevelMax, constructionlevelmax},
                {k.blockingRadius, blockingradius},
                {k.chargeAmount, chargeAmount},
                {k.inConnections, inConnections},
                {k.outConnections, outConnections},
                {k.coreTransferred, coreTransferred},
                {k.transferEfficiency, transferEfficiency},
                {k.productionLevel, productionLevel},
                {k.productionUpgradeAmount, productionUpgradeAmount},
                {k.coreConsumption, _coreConsumption},
                {k.effectType, effectId},
                {k.coreCalories,_coreCalories},
                {k.coreKickStartThreshold, coreKickStartThreshold},
                {k.reinforceCounterMax, reinforceCounterMax},
                {k.bandwidthUsage, bandwidthUsage},
                {k.bandwidthCapacity, bandwidthCapacity},
                {k.emitRadius, emitRadius},
                {k.typeExclusiveRange, typeExclusiveRange},
                {k.networkNodeRange, network_node_range},
            };
        }
    }
}
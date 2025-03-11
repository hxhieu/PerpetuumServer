using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.ExportedTypes;
using Perpetuum.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perpetuum.Zones.Effects.ZoneEffects
{
    /// <summary>
    /// DB querying object for ZoneEffects
    /// </summary>
    public class ZoneEffectReader(IDbRepository<Zoneeffect> zoneEffectRepo)
    {
        private ZoneEffect CreateZoneEffectFromRecord(Zoneeffect entity)
        {
            try
            {
                var zoneId = entity.Zoneid;
                var effectId = entity.Effectid;
                var effectType = EnumHelper.GetEnum<EffectType>(effectId);
                var config = new ZoneEffect(zoneId, effectType, true); // TODO new bool col for isPlayerOnly
                return config;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
            return null;
        }

        public IEnumerable<ZoneEffect> GetStaticZoneEffects(IZone zone)
        {
            return zoneEffectRepo.GetMany(e => e.Zoneid == zone.Id).Select(CreateZoneEffectFromRecord).Where(x => x != null);
        }
    }
}

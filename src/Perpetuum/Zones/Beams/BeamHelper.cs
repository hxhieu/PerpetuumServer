using Perpetuum.Data;
using Perpetuum.ExportedTypes;
using Perpetuum.Log;
using System.Collections.Generic;

namespace Perpetuum.Zones.Beams
{
    /// <summary>
    /// Helper functions to emit a beam into the zone
    /// </summary>
    public static class BeamHelper
    {
        private static readonly IDictionary<BeamType, int> _cacheBeamDelays = Database.CreateCache<BeamType, int, DataContext.Entities.Beam>(
            x => (BeamType)x.Id,
            x => x.Startdelay
        );
        private static readonly IDictionary<int, BeamType> _cacheBeamAssignments = Database.CreateCache<int, BeamType, DataContext.Entities.Beamassignment>(
            x => x.Definition,
            x => (BeamType)x.Beam
        );

        public static int GetBeamDelay(BeamType beamType)
        {
            return _cacheBeamDelays[beamType];
        }

        public static BeamType GetBeamByDefinition(int definition)
        {
            if (!_cacheBeamAssignments.ContainsKey(definition))
            {
                Logger.Warning("no beam defined for definition: " + definition);
                return BeamType.undefined;
            }

            return _cacheBeamAssignments[definition];
        }
    }
}
using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.Services.EventServices;
using Perpetuum.Services.RiftSystem;

namespace Perpetuum.Zones.NpcSystem
{
    public class NpcBossInfoBuilder
    {
        private readonly ICustomRiftConfigReader _customRiftConfigReader;
        private readonly EventListenerService _eventChannel;

        public NpcBossInfoBuilder(ICustomRiftConfigReader customRiftConfigReader, EventListenerService eventChannel, IDbRepository<Npcbossinfo> npcBossInfoRepo)
        {
            _customRiftConfigReader = customRiftConfigReader;
            _eventChannel = eventChannel;
        }

        public NpcBossInfo CreateBossInfoFromDB(Npcbossinfo entity)
        {
            int id = entity.Id;
            int flockid = entity.Flockid;
            double? respawnFactor = entity.RespawnNoiseFactor;
            bool lootSplit = entity.LootSplitFlag;
            long? outpostEID = entity.OutpostEid;
            int? stabilityPts = entity.StabilityPts;
            bool overrideRelations = entity.OverrideRelations;
            string deathMessage = entity.CustomAggressMessage;
            string aggressMessage = entity.CustomAggressMessage;
            int? riftConfigId = entity.RiftConfigId;
            CustomRiftConfig riftConfig = _customRiftConfigReader.GetById(riftConfigId ?? -1);
            bool announce = entity.IsAnnounced;
            bool isServerWideAnnouncement = entity.IsServerWideAnnouncement??false;
            bool isNoRadioDelay = entity.IsNoRadioDelay ??false;

            NpcBossInfo info = new(
                _eventChannel,
                id,
                flockid,
                respawnFactor,
                lootSplit,
                outpostEID,
                stabilityPts,
                overrideRelations,
                deathMessage,
                aggressMessage,
                riftConfig,
                announce,
                isServerWideAnnouncement,
                isNoRadioDelay
             );

            return info;
        }
    }
}

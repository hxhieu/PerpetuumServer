using Perpetuum.Accounting.Characters;
using Perpetuum.Items;
using System;
using System.Collections.Generic;

namespace Perpetuum.Services.MissionEngine.Missions
{
    public class MissionStandingChange
    {
        public readonly long allianceEid;
        public readonly double change;

        public static MissionStandingChange FromRecord(DataContext.Entities.Missionstandingchange entity)
        {
            var allianceEid = entity.Allianceeid ?? 0;
            var change = entity.Change;

            return new MissionStandingChange(allianceEid, change);

        }

        public MissionStandingChange(long allianceEid, double change)
        {
            this.allianceEid = allianceEid;
            this.change = change;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.allianceEID, allianceEid},
                {k.amount, change}
            };
        }
    }


    public class MissionReward
    {
        public ItemInfo ItemInfo { get; private set; }
        public int Probability { get; private set; }

        public MissionReward(ItemInfo itemInfo)
        {
            Probability = 100;
            ItemInfo = itemInfo;
        }

        private MissionReward(int definition, int quantity, int probability)
        {
            ItemInfo = new ItemInfo(definition, quantity);
            Probability = probability;
        }


        public static MissionReward FromRecord(DataContext.Entities.Missionreward entity)
        {
            var definition = entity.Definition;
            var quantity = entity.Quantity;
            var probability = entity.Probability;

            return new MissionReward(definition, quantity, probability);
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.definition, ItemInfo.Definition},
                {k.quantity, ItemInfo.Quantity},
                {k.probability, Probability}
            };
        }
    }


    public class MissionIssuer
    {
        public readonly long corporationEid;
        public readonly long allianceEid;

        public MissionIssuer(DataContext.Entities.Missionissuer record)
        {
            corporationEid = record.Corporationeid ?? 0;
            allianceEid = record.Allianceeid ?? 0;
        }
    }

    // %%% ezt a dolgot teljesen at kene alakitani. Most nincs hasznalva
    public class MissionStandingRequirement
    {
        private readonly long _corporationEid;
        private readonly bool _standingAbove;
        private readonly double _standingThreshold;

        public MissionStandingRequirement(DataContext.Entities.Missionrequiredstanding entity)
        {
            _corporationEid = entity.Corporationeid ?? 0;
            _standingAbove = entity.Standingabove;
            _standingThreshold = entity.Standingthreshold;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.corporationEID, _corporationEid},
                {k.standingAbove, _standingAbove ? 1 : 0},
                {k.standingThreshold, _standingThreshold}
            };
        }

        public bool CheckStanding(Character character)
        {
            return true;
        }
    }


    [Serializable]
    public struct MissionProgressUpdate
    {
        public Character character;
        public int missionId;
        public Guid missionGuid;
        public int targetOrder;
        public bool isFinished;
        public int missionLevel;
        public int locationId;
        public int selectedRace;
        public bool spreadInGang;


        public override string ToString()
        {
            return "missionProgressUpdate characterID:" + character.Id + " missionID:" + missionId + " TargetOrder:" + targetOrder + " isFinished:" + isFinished + " lvl:" + missionLevel + " loc:" + locationId;
        }
    }
}

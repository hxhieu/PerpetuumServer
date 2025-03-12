using Perpetuum.Groups.Alliances;
using Perpetuum.Services.MissionEngine.MissionDataCacheObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perpetuum.Services.MissionEngine.Missions
{
    public class MissionAgent
    {
        private static MissionDataCache _missionDataCache;

        private long _ownerEid;
        public long OwnerEid => OwnerAlliance?.Eid ?? _ownerEid;

        public static void Init(MissionDataCache missionDataCache)
        {
            _missionDataCache = missionDataCache;
        }

        // sqlbol kiszedni AgentEid,BaseEid

        public readonly int id;
        private readonly string _name;


        private readonly Lazy<List<Mission>> _myMissions;

        public Alliance OwnerAlliance { get; private set; }

        private IEnumerable<Mission> MyMissions
        {
            get { return _myMissions.Value; }
        }

        public IEnumerable<Mission> MyRandomMissions
        {
            get { return MyMissions.Where(m => m.behaviourType == MissionBehaviourType.Random && m.listable); }
        }

        public IEnumerable<Mission> MyConfigMissions
        {
            get { return MyMissions.Where(m => m.behaviourType == MissionBehaviourType.Config && m.listable); }
        }


        public override string ToString()
        {
            return string.Format("mission agent. id: " + id + " " + _name + " owner:" + OwnerAlliance.Name);
        }


        public MissionAgent(DataContext.Entities.Missionagent entity)
        {
            id = entity.Id;
            _name = entity.Agentname;
            _myMissions = new Lazy<List<Mission>>(CollectMyMissions);
            _ownerEid = entity.Owner ?? 0;
        }

        public void SetOwnerAlliance(Alliance alliance)
        {
            OwnerAlliance = alliance;
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                {k.ID, id},
                {k.name, _name},
            };
        }


        private List<Mission> CollectMyMissions()
        {
            var missionList = new List<Mission>();

            int[] missionIds;
            if (!_missionDataCache.GetMissionIdsByAgent(this, out missionIds))
            {
                return missionList;
            }

            foreach (var missionId in missionIds)
            {
                Mission mission;
                if (_missionDataCache.TryGetMissionById(missionId, out mission))
                {
                    missionList.Add(mission);
                }
            }

            return missionList;
        }


        public IEnumerable<Mission> GetConfigMissionsByCategoryAndLevel(MissionCategory missionCategory, int missionLevel)
        {
            return MyConfigMissions.Where(m => m.missionCategory == missionCategory && m.MissionLevel == missionLevel);
        }


    }
}

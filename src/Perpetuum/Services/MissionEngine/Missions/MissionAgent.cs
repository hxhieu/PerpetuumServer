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


        public static MissionAgent FromRecord(DataContext.Entities.Missionagent record)
        {
            var id = record.Id;
            var name = record.Agentname;
            var ownerEid = record.Owner ?? 0;

            var agent = new MissionAgent(id, name)
            {
                OwnerAlliance = Alliance.GetOrThrow(ownerEid)
            };

            return agent;
        }

        private MissionAgent(int id, string name)
        {
            this.id = id;
            _name = name;
            _myMissions = new Lazy<List<Mission>>(CollectMyMissions);
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

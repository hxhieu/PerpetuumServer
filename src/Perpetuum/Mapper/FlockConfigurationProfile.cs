using Perpetuum.DataContext.Entities;
using Perpetuum.EntityFramework;
using Perpetuum.Zones.NpcSystem.AI.Behaviors;
using Perpetuum.Zones.NpcSystem.Flocks;
using System;

namespace Perpetuum.Mapper
{
    internal class FlockConfigurationProfile : AutoMapper.Profile
    {
        public FlockConfigurationProfile()
        {
            //c.ID = r.GetValue<int>("id");
            //c.Name = r.GetValue<string>("name");
            //c.PresenceID = r.GetValue<int>("presenceid");
            //c.FlockMemberCount = r.GetValue<int>("flockmembercount");
            //c.EntityDefault = EntityDefault.Get(r.GetValue<int>("definition"));
            //c.SpawnOrigin = new Position(r.GetValue<int>("spawnoriginX"), r.GetValue<int>("spawnoriginY"));
            //c.SpawnRange = new IntRange(r.GetValue<int>("spawnrangeMin"), r.GetValue<int>("spawnrangeMax"));
            //c.RespawnTime = TimeSpan.FromSeconds(r.GetValue<int>("respawnseconds"));
            //c.TotalSpawnCount = r.GetValue<int>("totalspawncount");
            //c.HomeRange = r.GetValue<int>("homerange");
            //c.Note = r.GetValue<string>("note");
            //c.RespawnMultiplierLow = r.GetValue<double>("respawnmultiplierlow");
            //c.IsCallForHelp = r.GetValue<bool>("iscallforhelp");
            //c.Enabled = r.GetValue<bool>("enabled");
            //c.BehaviorType = (BehaviorType)r.GetValue<int>("behaviorType");
            //c.SpecialType = (NpcSpecialType)r.GetValue<int>("npcSpecialType");
            //c.BossInfo = _bossBuilder.GetBossInfoByFlockID(c.ID, c);

            CreateMap<Npcflock, FlockConfiguration>()
                .ForMember(dest => dest.EntityDefault, opt => opt.MapFrom(src => EntityDefault.Get(src.Definition)))
                .ForMember(dest => dest.SpawnOrigin, opt => opt.MapFrom(src => new Position(src.SpawnoriginX, src.SpawnoriginY, 0)))
                .ForMember(dest => dest.SpawnRange, opt => opt.MapFrom(src => new IntRange(src.SpawnrangeMin, src.SpawnrangeMax)))
                .ForMember(dest => dest.RespawnTime, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Respawnseconds)))
                .ForMember(dest => dest.BehaviorType, opt => opt.MapFrom(src => (BehaviorType)src.BehaviorType))
                .ForMember(dest => dest.SpecialType, opt => opt.MapFrom(src => (Zones.NpcSystem.NpcSpecialType)src.NpcSpecialType))
            ;
        }
    }
}

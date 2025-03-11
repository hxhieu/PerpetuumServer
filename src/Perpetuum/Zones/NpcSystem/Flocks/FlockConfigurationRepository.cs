using AutoMapper;
using Perpetuum.Data;
using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perpetuum.Zones.NpcSystem.Flocks
{
    public class FlockConfigurationRepository(
        FlockConfigurationBuilder.Factory flockConfigurationBuilderFactory,
        NpcBossInfoBuilder bossBuilder,
        IMapper mapper,
        IDbRepository<Npcflock> flockRepo,
        IDbRepositoryReadOnly<Npcbossinfo> bossRepo
    ) : IFlockConfigurationRepository
    {
        private readonly Dictionary<int, IFlockConfiguration> _flockConfigurations = [];

        public void LoadAllConfig()
        {
            var flocks = flockRepo.GetMany(x => x.Enabled, TimeSpan.FromHours(1));  // Can cache for a very long time?
            var bosses = bossRepo.GetMany(cacheTime: TimeSpan.FromHours(1));  // Can cache for a very long time?

            foreach (var r in flocks)
            {
                var builder = flockConfigurationBuilderFactory();

                builder.With(c =>
                {
                    c = mapper.Map(r, c);
                    var boss = bosses.FirstOrDefault(x => x.Flockid == x.Id);
                    if (boss != null)
                    {
                        c.BossInfo = bossBuilder.CreateBossInfoFromDB(boss);
                    }
                });

                var config = builder.Build();

                _flockConfigurations[config.ID] = config;
            }
        }

        public IEnumerable<IFlockConfiguration> GetAllByPresence(int presenceID)
        {
            return _flockConfigurations.Values.Where(t => t.PresenceID == presenceID).ToArray();
        }

        public IFlockConfiguration Get(int flockID)
        {
            return _flockConfigurations.GetOrDefault(flockID);
        }

        public IEnumerable<IFlockConfiguration> GetAll()
        {
            return _flockConfigurations.Values;
        }

        public void Insert(IFlockConfiguration item)
        {
            const string query = @"insert npcflock 
                                   (name,presenceid,flockmembercount,definition,spawnoriginX,spawnoriginY,spawnrangeMin,spawnrangeMax,respawnseconds,totalspawncount,homerange,note,respawnmultiplierlow) values 
                                   (@name,@presenceID,@flockMemberCount,@definition,@spawnOriginX,@spawnOriginY,@spawnRangeMin,@spawnRangeMax,@respawnSeconds,@totalSpawnCount,@homeRange,@note,@respawnMultiplierLow);
                                   select cast(scope_identity() as int)";

            var id = Db.Query().CommandText(query)
                .SetParameter("@name", item.Name)
                .SetParameter("@presenceID", item.PresenceID)
                .SetParameter("@flockMemberCount", item.FlockMemberCount)
                .SetParameter("@definition", item.EntityDefault.Definition)
                .SetParameter("@spawnOriginX", item.SpawnOrigin.intX)
                .SetParameter("@spawnOriginY", item.SpawnOrigin.intY)
                .SetParameter("@spawnRangeMin", item.SpawnRange.Min)
                .SetParameter("@spawnRangeMax", item.SpawnRange.Max)
                .SetParameter("@respawnSeconds", item.RespawnTime.Seconds)
                .SetParameter("@totalSpawnCount", item.TotalSpawnCount)
                .SetParameter("@homeRange", item.HomeRange)
                .SetParameter("@respawnMultiplierLow", item.RespawnMultiplierLow)
                .SetParameter("@note", item.Note)
                .ExecuteScalar<int>().ThrowIfEqual(0, ErrorCodes.SQLInsertError);

            if (item is FlockConfiguration fc)
            {
                fc.ID = id;
            }
        }

        public void Update(IFlockConfiguration item)
        {
            const string query = @"update npcflock 
                                      set [name]=@name,
                                          presenceid=@presenceID,
                                          flockmembercount=@flockMemberCount,
                                          definition=@definition,
                                          spawnoriginX=@spawnOriginX,
                                          spawnoriginY=@spawnOriginY,
                                          spawnrangeMin=@spawnRangeMin,
                                          spawnrangeMax=@spawnRangeMax,
                                          respawnseconds=@respawnSeconds,
                                          totalspawncount=@totalSpawnCount,
                                          homerange=@homeRange,
                                          respawnmultiplierlow=@respawnMultiplierLow
                                      where id=@ID";

            var res = Db.Query().CommandText(query)
                    .SetParameter("@ID", item.ID)
                    .SetParameter("@name", item.Name)
                    .SetParameter("@presenceID", item.PresenceID)
                    .SetParameter("@flockMemberCount", item.FlockMemberCount)
                    .SetParameter("@definition", item.EntityDefault.Definition)
                    .SetParameter("@spawnOriginX", item.SpawnOrigin.intX)
                    .SetParameter("@spawnOriginY", item.SpawnOrigin.intY)
                    .SetParameter("@spawnRangeMin", item.SpawnRange.Min)
                    .SetParameter("@spawnRangeMax", item.SpawnRange.Max)
                    .SetParameter("@respawnSeconds", item.RespawnTime.Seconds)
                    .SetParameter("@totalSpawnCount", item.TotalSpawnCount)
                    .SetParameter("@homeRange", item.HomeRange)
                    .SetParameter("@respawnMultiplierLow", item.RespawnMultiplierLow)
                    .SetParameter("@note", item.Note)
                    .ExecuteNonQuery();

            if (res == 0)
            {
                throw new PerpetuumException(ErrorCodes.SQLUpdateError);
            }
        }

        public void Delete(IFlockConfiguration item)
        {
            Db.Query().CommandText("delete npcflock where id=@ID")
                .SetParameter("@ID", item.ID)
                .ExecuteNonQuery().ThrowIfEqual(0, ErrorCodes.SQLDeleteError);
        }
    }
}
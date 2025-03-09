using Microsoft.Extensions.Logging;
using Perpetuum.Data;
using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using System;
using System.Linq;

namespace Perpetuum.Host
{
    public class HostRepository(
        ILoggerFactory loggerFactory,
        IDbRepository<Account> accounts,
        IDbRepository<Character> characters,
        IDbRepository<Channelmember> channelMembers,
        IDbRepository<Intrusionsite> intrusionSites,
        IDbRepository<Mission> missions,
        IDbRepository<Missiontargetsarchive> missionArchives,
        IDbRepository<Missionlog> missionLogs,
        IDbRepository<Entity> entities,
        IDbRepository<Zoneuserentity> zoneUserEntities,
        IDbRepository<Pbsconnection> pbsConnections,
        IDbRepository<Marketitem> marketItems
    )
    {
        private Lazy<ILogger> _loggerInstance => new(() => loggerFactory.CreateLogger(nameof(HostRepository)));
        private ILogger _logger => _loggerInstance.Value;

        public void InitServer()
        {
            // PROCEDURE [dbo].[initServer]
            _logger.LogWarning(">>>> CLEANING UP SERVER PREVIOUS SESSION - STARTED <<<<");

            var count = accounts.UpdateBatch(x => x.AccountId > 0, x => new Account { IsLoggedIn = false });
            _logger.LogInformation("{Count} accounts login states are cleaned up", count);

            count = characters.UpdateBatch(x => x.CharacterId > 0, x => new Character { InUse = false });
            _logger.LogInformation("{Count} characters login states are cleaned up", count);

            var allChars = characters.GetMany(x => x.CharacterId > 0);
            var allCharIds = allChars.Where(x => x.Active).Select(x => x.CharacterId);

            //TODO: EXEC dbo.deleteUnusedPublicChannels

            // Clean up character channels
            var inactiveChars = allChars.Where(x => !x.Active).Select(x => x.CharacterId);
            count = channelMembers.DeleteBatch(x => inactiveChars.Contains(x.Memberid));
            _logger.LogInformation("{Count} channel members got cleaned", count);

            count = intrusionSites.UpdateBatch(x => x.Intrusionstarttime != null && x.Intrusionstarttime <= DateTime.Now.AddMinutes(10), x => new Intrusionsite { Intrusionstarttime = null });
            _logger.LogInformation("{Count} channel intrusion sites got cleaned", count);

            // Mission logs
            var listableMissions = missions.GetMany(x => x.Listable).Select(x => x.Id);
            count = missionArchives.DeleteBatch(x => !listableMissions.Contains(x.Missionid));
            _logger.LogInformation("{Count} obsolete missiontargetsarchive got cleaned", count);
            count = missionLogs.DeleteBatch(x => !listableMissions.Contains(x.MissionId));
            _logger.LogInformation("{Count} obsolete missionlog got cleaned", count);

            count = missionArchives.DeleteBatch(x => inactiveChars.Contains(x.Characterid));
            _logger.LogInformation("{Count} inactive characters in missiontargetsarchive got cleaned", count);
            count = missionLogs.DeleteBatch(x => inactiveChars.Contains(x.CharacterId));
            _logger.LogInformation("{Count} inactive characters in missionlog got cleaned", count);

            count = missionArchives.DeleteBatch(x => !allCharIds.Contains(x.Characterid));
            _logger.LogInformation("{Count} unknown characters in missiontargetsarchive got cleaned", count);
            count = missionLogs.DeleteBatch(x => !allCharIds.Contains(x.CharacterId));
            _logger.LogInformation("{Count} unknown characters in missionlog got cleaned", count);

            // Entities
            count = zoneUserEntities.ExecuteNonQuerySql("DELETE dbo.zoneuserentities WHERE eid NOT IN (SELECT eid FROM entities)");
            _logger.LogInformation("{Count} unknown entities in zoneuserentities got cleaned", count);

            count = pbsConnections.ExecuteNonQuerySql("DELETE dbo.pbsconnections WHERE  (sourceeid NOT IN (SELECT eid FROM zoneuserentities)) OR (targeteid NOT IN (SELECT eid FROM zoneuserentities))");
            _logger.LogInformation("{Count} unknown pbsConnections got cleaned", count);

            count = marketItems.ExecuteNonQuerySql("DELETE dbo.marketitems WHERE isSell=1 AND isvendoritem=0 AND itemeid NOT IN (SELECT eid FROM dbo.entities)");
            _logger.LogInformation("{Count} unknown marketItems got cleaned", count);

            ////the current host has to clean up things in the onlinehost table, and other runtime tables
            //_ = Db.Query().CommandText("initServer").ExecuteNonQuery();

            //GlobalConfiguration globalConfiguration = container.Resolve<GlobalConfiguration>();
            //if (!string.IsNullOrEmpty(globalConfiguration.PersonalConfig))
            //{
            //    _ = Db.Query().CommandText(globalConfiguration.PersonalConfig).ExecuteNonQuery();
            //    Logger.Info("Personal sp executed:" + globalConfiguration.PersonalConfig);
            //}

            accounts.SaveChanges();
            characters.SaveChanges();
            channelMembers.SaveChanges();
            intrusionSites.SaveChanges();
            missions.SaveChanges();
            missionArchives.SaveChanges();
            missionLogs.SaveChanges();
            entities.SaveChanges();
            zoneUserEntities.SaveChanges();
            pbsConnections.SaveChanges();
            marketItems.SaveChanges();

            _logger.LogWarning(">>>> CLEANING UP SERVER PREVIOUS SESSION - DONE <<<<");
        }
    }
}

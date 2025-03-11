using Perpetuum.DataContext;
using Perpetuum.DataContext.Entities;
using Perpetuum.Threading.Process;
using System;

namespace Perpetuum.Host
{
    public class HostOnlineStateWriter(IHostStateService hostStateService, IDbRepository<Gameglobal> gameGlobalRepo) : Process
    {
        private void UpdateHostStateToDb()
        {
            gameGlobalRepo.UpdateBatch(_ => true, x => new Gameglobal
            {
                Lastonline = DateTime.Now
            });
        }

        public override void Stop()
        {
            UpdateHostStateToDb();
        }

        public override void Update(TimeSpan time)
        {
            if (hostStateService.State != HostState.Online)
                return;

            UpdateHostStateToDb();
        }
    }
}
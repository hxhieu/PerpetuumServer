using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Entities;

namespace Perpetuum.DataContext.Context
{
    public partial class PerpetuumDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Custom or missing PKs

            modelBuilder.Entity<Missiontargetsarchive>()
                .HasKey(x => new { x.Missionid, x.Characterid, x.Targetid });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Entities;

namespace Perpetuum.DataContext.Context
{
    public partial class PerpetuumDbContext : IPerpetuumDbContext { }

    public interface IPerpetuumDbContext
    {
        DbSet<Account> Accounts { get; set; }
    }
}

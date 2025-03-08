using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Models;

namespace Perpetuum.DataContext.Context
{
    public partial class PerpetuumDbContext : IPerpetuumDbContext { }

    public interface IPerpetuumDbContext
    {
        DbSet<Account> Accounts { get; set; }
    }
}

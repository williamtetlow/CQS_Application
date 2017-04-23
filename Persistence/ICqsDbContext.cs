using System.Data.Entity;
using Domain;

namespace Persistence
{
    public interface ICqsDbContext
    {
        DbSet<Order> Orders { get; set; }
    }
}

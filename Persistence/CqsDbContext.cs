using System.Data.Entity;
using Domain;

namespace Persistence
{
    [DbConfigurationType(typeof(CqsDbContextConfiguration))]
    public class CqsDbContext : DbContext, ICqsDbContext
    {   
        public CqsDbContext(string connectionString) : base(connectionString) {}

        // -- Add model relationships here
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(order => order.OrderLines)
                .WithRequired()
                .HasForeignKey(orderLine => orderLine.OrderId);
        }

        // -- DB Tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
    }
}

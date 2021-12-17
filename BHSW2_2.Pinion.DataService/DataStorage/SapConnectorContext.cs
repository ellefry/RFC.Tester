using Microsoft.EntityFrameworkCore;

namespace BHSW2_2.Pinion.DataService
{
    public class SapConnectorContext : DbContext
    {
        public SapConnectorContext(DbContextOptions<SapConnectorContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<SapRequest> SapRequests { get; set; }
        public DbSet<SapRequestHistory> SapRequestHistories { get; set; }
    }
}

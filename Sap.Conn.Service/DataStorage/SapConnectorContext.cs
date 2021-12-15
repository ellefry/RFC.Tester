using Sap.Conn.Service.Domains;
using Sap.Conn.Service.Migrations;
using System.Data.Entity;

namespace Sap.Conn.Service.DataStorage
{
    public class SapConnectorContext : DbContext
    {
        public SapConnectorContext() : base("SapConnector")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SapConnectorContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ProcessRequest> ProcessRequests { get; set; }
        public DbSet<ProcessRequestHistory> ProcessRequestHistories { get; set; }
    }
}

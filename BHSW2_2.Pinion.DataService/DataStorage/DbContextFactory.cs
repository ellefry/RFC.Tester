using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using BHSW2_2.Pinion.DataService;

namespace Bosch.Nexeed.MES.BlockManagement.Service.DataStorage
{
    public class DbContextFactory : IDesignTimeDbContextFactory<SapConnectorContext>
    {
        public SapConnectorContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var dbConfigurationSection = configuration.GetSection("Database");

            if ((dbConfigurationSection["ConnectionString"] == null) || (dbConfigurationSection["Type"] == null))
            {
                Console.WriteLine("ConnectionString or Type is missing for the Database connection");
            }
            else
            {
                Console.WriteLine($"Connected to {dbConfigurationSection["Type"]}");
            }

            var type = dbConfigurationSection["Type"];
            var connectionString = dbConfigurationSection["ConnectionString"];
            var optionsBuilder = new DbContextOptionsBuilder<SapConnectorContext>();

            if (type.Equals("mssql"))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
            {
                throw new ArgumentException("Database Configuration error. Didn't find the correct database type.");
            }

            return new SapConnectorContext(optionsBuilder.Options);
        }
    }
}

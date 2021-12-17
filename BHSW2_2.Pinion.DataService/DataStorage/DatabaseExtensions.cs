using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BHSW2_2.Pinion.DataService
{
    public static class DatabaseExtensions
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetRequiredService<SapConnectorContext>().Database.Migrate();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfigurationSection = configuration.GetSection("Database");

            if ((dbConfigurationSection["ConnectionString"] == null) || (dbConfigurationSection["Type"] == null))
            { 
              throw new ArgumentException("ConnectionString or Type is missing for the Database connection");
            }

            var type = dbConfigurationSection["Type"];
            var connectionString = dbConfigurationSection["ConnectionString"];

            Console.WriteLine($"Configured database type: {type}");
            Console.WriteLine($"Configured connection string: {connectionString}");

            if (type.Equals("mssql"))
            {
                services.AddDbContext<SapConnectorContext>(options => { options.UseSqlServer(connectionString); });
            }
            else
            {
                throw new ArgumentException("Database Configuration error. Didn't find the correct database type.");
            }
        }
    }
}
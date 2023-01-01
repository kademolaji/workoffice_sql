using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WorkOffice.Domain.Helpers
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(envName))
            {
                // If empty, use DEV as environment
                envName = "Development";
            }

            Console.WriteLine($"Building database for Env->[{envName}]");
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetParent(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName}.json", true);

            var config = builder.Build();
            var connstr = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseNpgsql(connstr, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new DataContext(optionsBuilder.Options);
        }

        static string GetParent(string path)
        {
            var directoryInfo = Directory.GetParent(path);
            return directoryInfo.FullName;
        }
    }
}

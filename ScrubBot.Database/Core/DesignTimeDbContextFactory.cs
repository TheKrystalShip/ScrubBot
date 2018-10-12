using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace ScrubBot.Database.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
                .AddJsonFile("settings.json", optional: false)
                .Build();

            DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>();

            string connectionString = configuration.GetConnectionString("SQLite");

            builder.UseSqlite(connectionString);

            return new DatabaseContext(builder.Options);
        }
    }
}

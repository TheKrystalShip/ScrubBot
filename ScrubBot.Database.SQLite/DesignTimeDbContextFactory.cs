using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Database.SQLite
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SQLiteContext>
    {
        public SQLiteContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();

            Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"));
#if DEBUG
            Configuration.AddFiles("settings.dev.json");
#else
            Configuration.AddFiles("settings.json");
#endif

            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            return new SQLiteContext(builder.Options);
        }
    }
}

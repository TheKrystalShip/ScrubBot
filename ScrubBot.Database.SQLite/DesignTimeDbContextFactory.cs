using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using ScrubBot.Database.SQLite;
using ScrubBot.Tools;

namespace ScrubBot.Database.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SQLiteContext>
    {
        public SQLiteContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();

            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            return new SQLiteContext(builder.Options);
        }
    }
}

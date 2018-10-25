using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using ScrubBot.Tools;

using System.IO;

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

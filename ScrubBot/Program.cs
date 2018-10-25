using Microsoft.EntityFrameworkCore;
using ScrubBot.Core;
using ScrubBot.Database;
using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Services;
using ScrubBot.Tools;

using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        private static Bot _scrubBot;

        public static async Task Main(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            SQLiteContext dbContext = new SQLiteContext(builder.Options);

            Container.Add(dbContext);
            Container.Add<ServiceHandler>();
            Container.Add<EventService>();

            // Container.Get<ServiceHandler>();

            await (_scrubBot = new Bot()).InitAsync(Configuration.Get("Bot:Token"));
        }
    }
}

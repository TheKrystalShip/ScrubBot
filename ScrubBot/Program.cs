using System;
using Microsoft.EntityFrameworkCore;
using ScrubBot.Core;
using ScrubBot.Database;
using ScrubBot.Handlers;
using ScrubBot.Services;
using ScrubBot.Tools;
using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        private static IBot _scrubBot;

        public static async Task Main(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            SQLiteContext dbContext = new SQLiteContext(builder.Options);

            Container.Add(dbContext);
            Container.Add<ServiceHandler>();
            Container.Get<EventService>().Init(10000);

            var today = DateTime.UtcNow;
            Container.Get<BirthdayService>().Init((new DateTime(today.Year, today.Month, today.Day).AddDays(1) - 
                                                   new DateTime(today.Year, today.Month, today.Day)).Add(TimeSpan.FromHours(7)).Milliseconds, 86400000‬); // Occurs every 24 hours, from the moment it hits 7AM the next day, after doing an initial call

            await (_scrubBot = new Bot()).InitAsync(Configuration.Get("Bot:Token"));
        }
    }
}

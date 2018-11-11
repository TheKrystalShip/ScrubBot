using Microsoft.EntityFrameworkCore;

using ScrubBot.Core;
using ScrubBot.Database;
using ScrubBot.Handlers;
using ScrubBot.Services;
using ScrubBot.Tools;

using System;
using System.Threading.Tasks;

namespace ScrubBot
{
    public class Startup
    {
        public Startup()
        {

        }

        public Startup ConfigureDatabase()
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            SQLiteContext dbContext = new SQLiteContext(builder.Options);
            Container.Add(dbContext);

            return this;
        }

        public Startup ConfigureServices()
        {
            Container.Add<ServiceHandler>();
            Container.Add<EventService>();
            Container.Add<BirthdayService>();

            ServiceHandler serviceHandler = Container.Get<ServiceHandler>();

            EventService eventService = Container.Get<EventService>();
            eventService.Trigger += serviceHandler.OnEventServiceTriggerAsync;
            eventService.Init(10000);

            BirthdayService birthdayService = Container.Get<BirthdayService>();
            birthdayService.Trigger += serviceHandler.OnBirthdayServiceTriggerAsync;
            birthdayService.Init(DateTime.UtcNow.Date.AddDays(1).AddHours(7).Millisecond, 86400000);

            return this;
        }

        public Startup ConfigureBot()
        {
            IBot _bot = new Bot();
            _bot.InitAsync(Configuration.Get("Bot:Token"));

            return this;
        }

        public async Task InitAsync()
        {
            await Task.Delay(-1);
        }
    }
}

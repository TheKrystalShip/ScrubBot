using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Core;
using ScrubBot.Database.SQLite;
using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Services;
using ScrubBot.Tools;

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

        public Startup ConfigureManagers()
        {
            Container.Add<IPrefixManager, PrefixManager>();
            Container.Add<IChannelManager, ChannelManager>();
            Container.Add<IGuildManager, GuildManager>();
            Container.Add<IRoleManager, RoleManager>();
            Container.Add<IUserManager, UserManager>();

            return this;
        }

        public Startup ConfigureServices()
        {
            ServiceHandler serviceHandler = Container.Get<ServiceHandler>();

            EventService eventService = Container.Get<EventService>();
            eventService.Trigger += serviceHandler.OnEventServiceTriggerAsync;
            eventService.Init(10000);

            BirthdayService birthdayService = Container.Get<BirthdayService>();
            birthdayService.Trigger += serviceHandler.OnBirthdayServiceTriggerAsync;
            birthdayService.Init(DateTime.UtcNow.Date.AddDays(1).AddHours(7).Millisecond, 86400000);

            return this;
        }

        public Startup ConfigureClient()
        {
            Bot _bot = new Bot();
            _bot.InitAsync(Configuration.Get("Bot:Token")).Wait();

            return this;
        }

        public async Task InitAsync()
        {
            await Task.Delay(-1);
        }
    }
}

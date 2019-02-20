using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Core;
using ScrubBot.Core.Commands;
using ScrubBot.Database;
using ScrubBot.Database.SQLite;
using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Services;
using ScrubBot.Tools;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Tools.Configuration;

namespace ScrubBot
{
    public class Startup
    {
        public Startup()
        {
            Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"));
            #if DEBUG
            Configuration.AddFiles("settings.dev.json");
            #else
            Configuration.AddFiles("settings.json");
            #endif
        }

        public Startup ConfigureDatabase()
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            SQLiteContext dbContext = new SQLiteContext(builder.Options);
            dbContext.Migrate();

            Container.Add<IDbContext>(dbContext);

            return this;
        }

        public Startup ConfigureContainer()
        {
            Container.Add<IMiddlewareManager, MiddlewareManager>();
            Container.Add<IChannelManager, ChannelManager>();
            Container.Add<IGuildManager, GuildManager>();
            Container.Add<IPrefixManager, PrefixManager>();
            Container.Add<IRoleManager, RoleManager>();
            Container.Add<IUserManager, UserManager>();
            Container.Add<IManager, Manager>();

            Container.Add<ServiceHandler>();
            Container.Add<EventService>();
            Container.Add<BirthdayService>();

            return this;
        }

        public Startup ConfigureServices()
        {
            ServiceHandler serviceHandler = Container.Get<ServiceHandler>();

            EventService eventService = Container.Get<EventService>();
            eventService.Trigger += serviceHandler.OnEventServiceTriggerAsync;
            eventService.Init(TimeSpan.FromMinutes(1).Milliseconds, TimeSpan.FromSeconds(30).Milliseconds);

            BirthdayService birthdayService = Container.Get<BirthdayService>();
            birthdayService.Trigger += serviceHandler.OnBirthdayServiceTriggerAsync;
            birthdayService.Init(DateTime.UtcNow.Date.AddDays(1).AddHours(7).Millisecond, TimeSpan.FromDays(1).Milliseconds);

            return this;
        }

        public Startup ConfigureClient()
        {
            Bot client = new Bot(new DiscordSocketConfig()
            {
                ConnectionTimeout = 5000,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                HandlerTimeout = 1000,
                LogLevel = LogSeverity.Debug
            });

            CommandOperator commandOperator = new CommandOperator(client, new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Debug
            });

            commandOperator.CommandExecuted += Dispatcher.Dispatch;
            commandOperator.Log += Logger.Log;
            client.Log += Logger.Log;

            client.MessageReceived += commandOperator.OnClientMessageReceivedAsync;
            client.InitAsync(Configuration.Get("Bot:Token")).Wait();

            Container.Add(client);
            Container.Add<ICommandOperator>(commandOperator);

            commandOperator.LoadModulesAsync().Wait();

            return this;
        }

        public Startup ConfigureEvents()
        {
            Bot client = Container.Get<Bot>();
            IManager manager = Container.Get<IManager>();

            client.ChannelCreated += manager.Channels.OnChannelCreatedAsync;
            client.ChannelDestroyed += manager.Channels.OnChannelDestroyedAsync;
            client.ChannelUpdated += manager.Channels.OnChannelUpdatedAsync;

            client.GuildAvailable += manager.Guilds.OnGuildAvailableAsync;
            client.GuildMembersDownloaded += manager.Guilds.OnGuildMembersDownloadedAsync;
            client.GuildMemberUpdated += manager.Guilds.OnGuildMemberUpdatedAsync;
            client.GuildUnavailable += manager.Guilds.OnGuildUnavailableAsync;
            client.GuildUpdated += manager.Guilds.OnGuildUpdatedAsync;
            client.JoinedGuild += manager.Guilds.OnJoinedGuildAsync;
            client.LeftGuild += manager.Guilds.OnLeftGuildAsync;

            client.RoleCreated += manager.Roles.OnRoleCreatedAsync;
            client.RoleDeleted += manager.Roles.OnRoleDeletedAsync;
            client.RoleUpdated += manager.Roles.OnRoleUpdatedAsync;

            client.Ready += async () =>
            {
                await manager.Guilds.AddGuildsAsync(client.Guilds);
                await manager.Users.AddUsersAsync(client.Guilds);
            };

            return this;
        }

        public async Task InitAsync()
        {
            await Task.Delay(-1);
        }
    }
}

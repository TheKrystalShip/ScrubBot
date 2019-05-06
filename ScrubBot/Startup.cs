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
    /// <summary>
    /// Responsible for loading, configuring and starting all the
    /// required parts of the project for the Bot client to work.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Load settings.json (or settings.dev.json) files into memory
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureSettings()
        {
            Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"));
#if DEBUG
            Configuration.AddFiles("settings.dev.json");
#else
            Configuration.AddFiles("settings.json");
#endif

            return this;
        }

        /// <summary>
        /// Configure database access and migrate schema
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureDatabase()
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));

            SQLiteContext dbContext = new SQLiteContext(builder.Options);
            dbContext.Migrate();

            Container.Add<IDbContext>(dbContext);

            return this;
        }

        /// <summary>
        /// Load up needed dependencies into the IoC Container
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureContainer()
        {
            Container.Add<IChannelManager, ChannelManager>();
            Container.Add<IGuildManager, GuildManager>();
            Container.Add<IPrefixManager, PrefixManager>();
            Container.Add<IRoleManager, RoleManager>();
            Container.Add<IUserManager, UserManager>();
            Container.Add<IReactionManager, ReactionManager>();
            Container.Add<IManager, Manager>();

            Container.Add<ServiceHandler>();
            Container.Add<EventService>();
            Container.Add<BirthdayService>();

            return this;
        }

        /// <summary>
        /// Configure background services
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureServices()
        {
            ServiceHandler serviceHandler = Container.Get<ServiceHandler>();

            // In 1m
            int startupDelay = TimeSpan.FromMinutes(1).Milliseconds;

            // Every 30s
            int interval = TimeSpan.FromSeconds(30).Milliseconds;

            EventService eventService = Container.Get<EventService>();
            eventService.Trigger += serviceHandler.OnEventServiceTriggerAsync;
            eventService.Init(startupDelay, interval);

            // Next day @ 07:00
            startupDelay = DateTime.UtcNow.Date.AddDays(1).AddHours(7).Millisecond;

            // Every 24H
            interval = TimeSpan.FromDays(1).Milliseconds;

            BirthdayService birthdayService = Container.Get<BirthdayService>();
            birthdayService.Trigger += serviceHandler.OnBirthdayServiceTriggerAsync;
            birthdayService.Init(startupDelay, interval);

            return this;
        }

        /// <summary>
        /// Configure and start bot client
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureClient()
        {
#if DEBUG
            DiscordSocketConfig discordSocketConfig = new DiscordSocketConfig
            {
                ConnectionTimeout = 5000,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                HandlerTimeout = 1000,
                LogLevel = LogSeverity.Debug
            };

            CommandServiceConfig commandServiceConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Debug
            };
#else
            DiscordSocketConfig config = new DiscordSocketConfig
            {
                ConnectionTimeout = 1000,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                HandlerTimeout = 1000,
                LogLevel = LogSeverity.Info
            };

            CommandServiceConfig commandServiceConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Info
            };
#endif

            Bot client = new Bot(discordSocketConfig);
            CommandOperator commandOperator = new CommandOperator(client, commandServiceConfig);

            Container.Add(client);
            Container.Add(commandOperator);

            return this;
        }

        /// <summary>
        /// Hook into the Bot event
        /// </summary>
        /// <returns></returns>
        public Startup ConfigureEvents()
        {
            Bot client = Container.Get<Bot>();
            CommandOperator commandOperator = Container.Get<CommandOperator>();
            IManager manager = Container.Get<IManager>();

            client.Log += Logger.Log;
            client.MessageReceived += commandOperator.OnClientMessageReceivedAsync;

            client.ChannelCreated += manager.Channels.OnChannelCreatedAsync;
            client.ChannelDestroyed += manager.Channels.OnChannelDestroyedAsync;
            client.ChannelUpdated += manager.Channels.OnChannelUpdatedAsync;

            client.GuildAvailable += manager.Guilds.OnGuildAvailableAsync;
            client.GuildMembersDownloaded += manager.Guilds.OnGuildMembersDownloadedAsync;
            //client.GuildMemberUpdated += manager.Guilds.OnGuildMemberUpdatedAsync;
            client.GuildUnavailable += manager.Guilds.OnGuildUnavailableAsync;
            client.GuildUpdated += manager.Guilds.OnGuildUpdatedAsync;
            client.JoinedGuild += manager.Guilds.OnJoinedGuildAsync;
            client.LeftGuild += manager.Guilds.OnLeftGuildAsync;

            client.RoleCreated += manager.Roles.OnRoleCreatedAsync;
            client.RoleDeleted += manager.Roles.OnRoleDeletedAsync;
            client.RoleUpdated += manager.Roles.OnRoleUpdatedAsync;

            client.ReactionAdded += manager.Reactions.OnReactionAdded;
            client.ReactionRemoved += manager.Reactions.OnReactionRemoved;

            client.Ready += () =>
            {
                _ = manager.Guilds.AddGuildsAsync(client.Guilds);
                _ = manager.Users.AddUsersAsync(client.Guilds);
                return Task.CompletedTask;
            };

            commandOperator.CommandExecuted += Dispatcher.Dispatch;
            commandOperator.Log += Logger.Log;

            return this;
        }

        /// <summary>
        /// Prevent program from exiting
        /// </summary>
        /// <returns></returns>
        public async Task InitAsync()
        {
            Bot client = Container.Get<Bot>();
            await client.InitAsync(Configuration.Get("Bot:Token"));

            CommandOperator commandOperator = Container.Get<CommandOperator>();
            await commandOperator.LoadModulesAsync();

            await Task.Delay(-1);
        }
    }
}

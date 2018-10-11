using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Database;
using ScrubBot.Extensions;
using ScrubBot.Properties;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ScrubBot.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        private PrefixHandler _prefixHandler;
        
        public CommandHandler(ref DiscordSocketClient client)
        {
            _client = client;

            _commandService = new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Debug
            });

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly()).Wait();

            // Register everything into the IoC container, so that it
            // can be called upon later
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddHandlers() // See ServiceCollectionExtensions.cs
                .AddServices() // See ServiceCollectionExtensions.cs
                .AddDbContext<DatabaseContext>(options => {
                    options.UseSqlite(Settings.Instance.GetConnectionString("SQLite"));
                })
                .BuildServiceProvider();

            // Launch any necessary services here, DI takes care of injection
            // since the DiscordSocketClient was registered into the container,
            // it will get injected into the constructors automatically.
            _serviceProvider.GetRequiredService<EventHandler>();
            _serviceProvider.GetRequiredService<ServiceHandler>();
            _prefixHandler = _serviceProvider.GetRequiredService<PrefixHandler>();

            _commandService.Log += CommandServiceLog;
            _client.MessageReceived += HandleCommand;
        }

        private Task CommandServiceLog(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        private string GetCharPrefix(SocketUserMessage message)
        {
            try
            {
                SocketGuildChannel guildChannel = message.Channel as SocketGuildChannel;
                ulong socketGuildId = guildChannel.Guild.Id;
                return _prefixHandler.GetCharPrefix(socketGuildId);
            }
            catch (Exception e)
            {
                //LogHandler.WriteLine(LogTarget.Console, e);
                Console.WriteLine(e);
                return null;
            }
        }

        private string GetStringPrefix(SocketUserMessage message)
        {
            try
            {
                var guildChannel = message.Channel as SocketGuildChannel;
                ulong socketGuildId = guildChannel.Guild.Id;
                return _prefixHandler.GetStringPrefix(socketGuildId);
            }
            catch (Exception e)
            {
                //LogHandler.WriteLine(LogTarget.Console, e);
                Console.WriteLine(e);
                return null;
            }
        }

        private async Task HandleCommand(SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            string charPrefix = GetCharPrefix(message) ?? Settings.Instance["Prefix:DefaultChar"];
            string stringPrefix = GetStringPrefix(message) ?? Settings.Instance["Prefix:DefaultString"];
            int argPos = 0;

            bool hasCharPrefix = message.HasCharPrefix(charPrefix.ToCharArray()[0], ref argPos);
            bool hasStringPrefix = message.HasStringPrefix(stringPrefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasCharPrefix && !hasStringPrefix && !isMentioned)
                return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

            if (!result.IsSuccess)
            {
                Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
            }
        }
    }
}
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

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddHandlers()
                .AddServices()
                .AddTools()
                .AddDbContext<DatabaseContext>(options => {
                    options.UseSqlite(Settings.Instance.GetConnectionString("SQLite"));
                })
                .BuildServiceProvider();

            _serviceProvider.GetRequiredService<EventHandler>();
            _serviceProvider.GetRequiredService<ServiceHandler>();
            _prefixHandler = _serviceProvider.GetRequiredService<PrefixHandler>();
            _serviceProvider.GetRequiredService<DatabaseContext>().MigrateDatabase();

            _commandService.Log += CommandServiceLog;
            _client.MessageReceived += HandleCommand;
        }

        private Task CommandServiceLog(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        private string GetPrefix(SocketUserMessage message)
        {
            try
            {
                var guildChannel = message.Channel as SocketGuildChannel;
                ulong socketGuildId = guildChannel.Guild.Id;
                return _prefixHandler.Get(socketGuildId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private async Task HandleCommand(SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            string stringPrefix = GetPrefix(message) ?? Settings.Instance["Prefix:DefaultString"];
            int argPos = 0;
            
            bool hasStringPrefix = message.HasStringPrefix(stringPrefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasStringPrefix && !isMentioned)
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
using System;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceProvider _serviceCollection;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _commandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            //_commandService.AddModuleAsync(Assembly.GetEntryAssembly());
        }
    }
}
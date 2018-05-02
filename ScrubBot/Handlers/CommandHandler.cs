using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ScrubBot.Handlers
{
    public class CommandHandler
    {
        private char _commandPrefix = '#';

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceCollection;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _commandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            _serviceCollection = new ServiceCollection().BuildServiceProvider();

            Console.Title = "ScrubBot";
            _client.MessageReceived += HandleCommand;
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;
            
            int argPos = 0;
            bool hasCharPrefix = message.HasCharPrefix(_commandPrefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasCharPrefix && !isMentioned) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceCollection);

            if (result.IsSuccess) return;
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
        }
    }
}
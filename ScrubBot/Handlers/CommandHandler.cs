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

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            _serviceCollection = new ServiceCollection().BuildServiceProvider();

            Console.Title = @"ScrubBot";
            _client.MessageReceived += HandleCommand;
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;
            bool mention = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!mention) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceCollection);

            if (result.IsSuccess) return;
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
        }
    }
}
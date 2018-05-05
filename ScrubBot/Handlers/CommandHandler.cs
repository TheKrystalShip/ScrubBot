using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Data;

namespace ScrubBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceProvider _serviceCollection;
        private DatabaseContext _dbContext;

        public CommandHandler(DiscordSocketClient client) => Initialize(client).Wait();

        private async Task Initialize(DiscordSocketClient client)
        {
            _client = client;

            _commandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            _serviceCollection = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddDbContext<DatabaseContext>()
                .BuildServiceProvider();

            Console.Title = @"ScrubBot";
            _client.MessageReceived += HandleCommand;
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;
            var guildChannel = (SocketGuildChannel)message.Channel;
            string socketGuildId = guildChannel.Guild.Id.ToString();

            int argPos = 0;
            bool hasCharPrefix = message.HasCharPrefix(PrefixHandler.GetCharPrefix(socketGuildId).ToCharArray()[0], ref argPos);
            bool hasStringPrefix = message.HasStringPrefix(PrefixHandler.GetStringPrefix(socketGuildId), ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasCharPrefix && !hasStringPrefix && !isMentioned) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceCollection);

            if (result.IsSuccess) return;
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
        }
    }
}
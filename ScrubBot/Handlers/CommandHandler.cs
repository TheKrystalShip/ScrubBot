using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Properties;

namespace ScrubBot.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceProvider _serviceCollection;

        public CommandHandler(DiscordSocketClient client) => Initialize(client).Wait();

        private async Task Initialize(DiscordSocketClient client)
        {
            _client = client;

            _commandService = new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
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

        private string GetCharPrefix(SocketUserMessage message)
        {
            try
            {
                var guildChannel = message.Channel as SocketGuildChannel;
                string socketGuildId = guildChannel.Guild.Id.ToString();
                return PrefixHandler.GetCharPrefix(socketGuildId);
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
                string socketGuildId = guildChannel.Guild.Id.ToString();
                return PrefixHandler.GetStringPrefix(socketGuildId);
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
            var message = msg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;

            string charPrefix = GetCharPrefix(message) ?? Resources.DefaultCharPrefix;
            string stringPrefix = GetStringPrefix(message) ?? Resources.DefaultStringPrefix;
            int argPos = 0;

            bool hasCharPrefix = message.HasCharPrefix(charPrefix.ToCharArray()[0], ref argPos);
            bool hasStringPrefix = message.HasStringPrefix(stringPrefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!hasCharPrefix && !hasStringPrefix && !isMentioned) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceCollection);

            if (result.IsSuccess) return;
            Console.WriteLine(new LogMessage(LogSeverity.Error, "Command", result.ErrorReason));
        }
    }
}
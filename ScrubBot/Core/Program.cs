using Discord;
using Discord.WebSocket;

using ScrubBot.Handlers;
using ScrubBot.Properties;

using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        // Only one instance per app lifetime
        private static DiscordSocketClient _client;
        private static CommandHandler _commandHandler;
        private static readonly string _token = Resources.LoginToken;

        public static async Task Main(string[] args)
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                AlwaysDownloadUsers = true
            });

            // Just reference the _client, no need to copy it
            _commandHandler = new CommandHandler(ref _client);

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");

            await Task.Delay(-1);
        }
    }
}
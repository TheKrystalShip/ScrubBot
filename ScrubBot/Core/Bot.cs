using Discord;
using Discord.WebSocket;

using ScrubBot.Handlers;
using ScrubBot.Properties;

using System.Threading.Tasks;

namespace ScrubBot
{
    public class Bot
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly string _token;

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                AlwaysDownloadUsers = true
            });

            _commandHandler = new CommandHandler(ref _client);

            _token = Resources.LoginToken;
        }

        public async Task InitAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync("Type >Help for help");
        }
    }
}

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;
using ScrubBot.Handlers;

namespace ScrubBot
{
    public class Program
    {
        private string _token;
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        private EventHandler _eventHandler;

        static void Main(string[] args) => new Program().Initialize().Wait();

        private async Task Initialize()
        {
            _token = Properties.Resources.LoginToken;
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true
            });

            _commandHandler = new CommandHandler(_client);
            _eventHandler = new EventHandler(_client);

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync(Properties.Resources.ActiveGame);

            await Task.Delay(-1);
        }
    }
}
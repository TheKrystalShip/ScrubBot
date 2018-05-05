using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using ScrubBot.Handlers;
using ScrubBot.Properties;

namespace ScrubBot
{
    public class Program
    {
        private string _token;
        private DiscordSocketClient _client;

        private CommandHandler _commandHandler;
        private Handlers.EventHandler _eventHandler;
        private ServiceHandler _serviceHandler;
        private PrefixHandler _prefixHandler;

        static void Main(string[] args) => new Program().Initialize().Wait();

        private async Task Initialize()
        {
            _token = Resources.LoginToken;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                AlwaysDownloadUsers = true
            });

            _commandHandler = new CommandHandler(_client);
            _eventHandler = new EventHandler(_client);
            _serviceHandler = new ServiceHandler(_client);
            _prefixHandler = new PrefixHandler();

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync("some scrub sh*t");

            await Task.Delay(-1);
        }
    }
}
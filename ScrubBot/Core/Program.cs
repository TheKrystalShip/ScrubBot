using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using ScrubBot.Handlers;
using ScrubBot.Properties;

namespace ScrubBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private readonly string _token = Resources.LoginToken;

        private CommandHandler _commandHandler;
        private EventHandler _eventHandler;
        private ServiceHandler _serviceHandler;
        private PrefixHandler _prefixHandler;

        static void Main(string[] args) => new Program().Initialize().Wait();

        private async Task Initialize()
        {
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
            await _client.SetGameAsync("Type >Help for help");

            await Task.Delay(-1);
        }
    }
}
using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        private string _token;
        private DiscordSocketClient _client;

        //private EventHandler _eventHandler;

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

            //_eventHandler = new EventHandler();

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync(Properties.Resources.ActiveGame);

            await Task.Delay(-1);
        }
    }
}
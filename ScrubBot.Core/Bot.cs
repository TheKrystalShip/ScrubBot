using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace ScrubBot.Core
{
    public class Bot : DiscordSocketClient
    {
        public Bot(DiscordSocketConfig config) : base(config)
        {

        }

        public async Task InitAsync(string token)
        {
            await LoginAsync(TokenType.Bot, token);
            await StartAsync();
            await SetGameAsync("Type >Help for help");
        }
    }
}

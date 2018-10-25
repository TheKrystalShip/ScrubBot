using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Core
{
    public class Context : SocketCommandContext
    {
        public Context(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
            
        }
    }
}

using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Modules
{
    public class Context : SocketCommandContext
    {
        public Context(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
            
        }
    }
}

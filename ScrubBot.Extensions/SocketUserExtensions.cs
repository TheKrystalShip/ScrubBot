using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ScrubBot.Extensions
{
    public static class SocketUserExtensions
    {
        public static Task<IUserMessage> SendMessageAsync(this SocketUser socketUser, EmbedBuilder embedBuilder)
        {
            return socketUser.SendMessageAsync(text: string.Empty, isTTS: false, embed: embedBuilder.Build());
        }

        public static Task<IUserMessage> SendMessageAsync(this SocketUser socketUser, Embed embed)
        {
            return socketUser.SendMessageAsync(text: string.Empty, isTTS: false, embed: embed);
        }
    }
}

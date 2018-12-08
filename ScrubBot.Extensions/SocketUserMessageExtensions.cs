using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Extensions
{
    public static class SocketUserMessageExtensions
    {
        public static bool IsValid(this SocketUserMessage message, in string prefix, in IUser user, out int argPos)
        {
            argPos = 0;

            if (message is null || message.Author.IsBot)
            {
                return false;
            }

            bool hasPrefix = message.HasStringPrefix(prefix, ref argPos);
            bool isMentioned = message.HasMentionPrefix(user, ref argPos);

            if (!hasPrefix && !isMentioned)
            {
                return false;
            }

            return true;
        }
    }
}

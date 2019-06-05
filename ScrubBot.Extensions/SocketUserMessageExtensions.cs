using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ScrubBot.Extensions
{
    public static class SocketUserMessageExtensions
    {
        public static bool IsGuildMessage(this SocketUserMessage message, in string prefix, in IUser user, out int argPos)
        {
            argPos = 0;
            bool hasPrefix = false;
            bool isMentioned = false;

            if (message is null || message.Author.IsBot)
            {
                return false;
            }

            if (prefix != null)
            {
                hasPrefix = message.HasStringPrefix(prefix, ref argPos);
            }

            if (user != null)
            {
                isMentioned = message.HasMentionPrefix(user, ref argPos);
            }

            if (!hasPrefix && !isMentioned)
            {
                return false;
            }

            return true;
        }

        public static bool IsPrivateMessage(this SocketUserMessage message, out int argPos)
        {
            argPos = 0;
            return message.Channel is IPrivateChannel;
        }
    }
}

using Discord.WebSocket;
using ScrubBot.Domain;

namespace ScrubBot.Extensions
{
    public static class SocketGuildExtensions
    {
        public static Guild ToGuild(this SocketGuild socketGuild)
        {
            return new Guild
            {
                Id = socketGuild.Id,
                Name = socketGuild.Name,
                IconUrl = socketGuild.IconUrl,
                MemberCount = socketGuild.MemberCount
            };
        }
    }
}

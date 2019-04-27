using Discord.WebSocket;

using ScrubBot.Database.Domain;

namespace ScrubBot.Extensions
{
    public static class SocketGuildUserExtensions
    {
        public static User ToUser(this SocketGuildUser socketGuildUser)
        {
            return new User
            {
                Username = socketGuildUser.Username,
                Id = socketGuildUser.Id,
                Nickname = socketGuildUser.Nickname,
                AvatarUrl = socketGuildUser.GetAvatarUrl(),
                Discriminator = socketGuildUser.Discriminator
            };
        }
    }
}

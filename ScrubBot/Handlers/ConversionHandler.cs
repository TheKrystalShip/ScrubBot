using Discord.WebSocket;
using System.Linq;
using ScrubBot.Database;
using ScrubBot.Database.Models;

namespace ScrubBot.Handlers
{
    public class ConversionHandler
    {
        private static readonly DatabaseContext db;
        public static int UsersAdded = 0;

        static ConversionHandler() => db = new DatabaseContext();

        public static void AddUser(SocketGuildUser socketGuildUser)
        {
            string socketUserId = socketGuildUser.Id.ToString();

            if (db.Users.Any(x => x.Id == socketUserId)) return;
            
            User user = new User
            {
                Username = socketGuildUser.Username,
                Id = socketUserId,
                Nickname = socketGuildUser.Nickname,
                AvatarUrl = socketGuildUser.GetAvatarUrl(),
                Discriminator = socketGuildUser.Discriminator,
                Guild = ToGuild(socketGuildUser.Guild)
            };

            db.Users.Add(user);
            db.SaveChanges();
            UsersAdded++;
        }

        public static void RemoveUser(SocketGuildUser user)
        {
            string userId = user.Id.ToString();
            if (!db.Users.Any(x => x.Id == userId)) return;

            User userToRemove = db.Users.FirstOrDefault(x => x.Id == userId);
            db.Users.Remove(userToRemove);
            db.SaveChanges();
        }

        private static Guild ToGuild(SocketGuild socketGuild)
        {
            string socketGuildId = socketGuild.Id.ToString();
            return db.Guilds.FirstOrDefault(x => x.Id == socketGuildId) ??
                   new Guild
                   {
                       Name = socketGuild.Name,
                       IconUrl = socketGuild.IconUrl,
                       Id = socketGuild.Id.ToString(),
                       MemberCount = socketGuild.MemberCount
                   };
        }
    }
}
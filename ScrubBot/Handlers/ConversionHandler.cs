using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database;
using ScrubBot.Database.Models;

using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Handlers
{
    public class ConversionHandler
    {
        private readonly DatabaseContext _db;
        public static int UsersAdded = 0;

        public ConversionHandler(DatabaseContext dbContext)
        {
            _db = dbContext;
        }

        public void AddUser(SocketGuildUser socketGuildUser)
        {
            string socketUserId = socketGuildUser.Id.ToString();

            if (_db.Users.Any(x => x.Id == socketUserId))
                return;
            
            User user = new User
            {
                Username = socketGuildUser.Username,
                Id = socketUserId,
                Nickname = socketGuildUser.Nickname,
                AvatarUrl = socketGuildUser.GetAvatarUrl(),
                Discriminator = socketGuildUser.Discriminator,
                Guild = ToGuild(socketGuildUser.Guild)
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            UsersAdded++;
        }

        public async Task AddUserAsync(SocketGuildUser socketGuildUser)
        {
            string socketUserId = socketGuildUser.Id.ToString();

            if (_db.Users.Any(x => x.Id == socketUserId))
                return;

            User user = new User
            {
                Username = socketGuildUser.Username,
                Id = socketUserId,
                Nickname = socketGuildUser.Nickname,
                AvatarUrl = socketGuildUser.GetAvatarUrl(),
                Discriminator = socketGuildUser.Discriminator,
                Guild = ToGuild(socketGuildUser.Guild)
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            UsersAdded++;
        }

        public void RemoveUser(SocketGuildUser user)
        {
            string userId = user.Id.ToString();

            if (!_db.Users.Any(x => x.Id == userId))
                return;

            User userToRemove = _db.Users.FirstOrDefault(x => x.Id == userId);
            _db.Users.Remove(userToRemove);
            _db.SaveChanges();
        }

        public async Task RemoveUserAsync(SocketGuildUser user)
        {
            string userId = user.Id.ToString();

            if (!_db.Users.Any(x => x.Id == userId))
                return;

            User userToRemove = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _db.Users.Remove(userToRemove);
            await _db.SaveChangesAsync();
        }

        private Guild ToGuild(SocketGuild socketGuild)
        {
            string socketGuildId = socketGuild.Id.ToString();
            return _db.Guilds.FirstOrDefault(x => x.Id == socketGuildId) ??
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
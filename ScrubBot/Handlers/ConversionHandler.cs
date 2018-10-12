using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Extensions;

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
            if (_db.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser(); // See .\Extensions\SocketGuildUserExtensions.cs
            user.Guild = ToGuild(socketGuildUser.Guild);

            _db.Users.Add(user);
            _db.SaveChanges();

            UsersAdded++;
        }

        public async Task AddUserAsync(SocketGuildUser socketGuildUser)
        {
            if (_db.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser(); // See .\Extensions\SocketGuildUserExtensions.cs
            user.Guild = ToGuild(socketGuildUser.Guild);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            UsersAdded++;
        }

        public void RemoveUser(SocketGuildUser user)
        {
            // .FirstOrDefault returns null is no value is found.
            User userToRemove = _db.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToRemove is null)
                return;

            _db.Users.Remove(userToRemove);
            _db.SaveChanges();
        }

        public async Task RemoveUserAsync(SocketGuildUser user)
        {
            // .FirstOrDefault returns null is no value is found.
            User userToRemove = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userToRemove is null)
                return;

            _db.Users.Remove(userToRemove);
            await _db.SaveChangesAsync();
        }

        private Guild ToGuild(SocketGuild socketGuild)
        {
            return _db.Guilds.FirstOrDefault(x => x.Id == socketGuild.Id) ??
                   new Guild
                   {
                       Name = socketGuild.Name,
                       IconUrl = socketGuild.IconUrl,
                       Id = socketGuild.Id,
                       MemberCount = socketGuild.MemberCount
                   };
        }
    }
}
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database;
using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Handlers
{
    public class ConversionHandler
    {
        private readonly SQLiteContext _dbContext;
        public static int UsersAdded = 0;

        public ConversionHandler(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(SocketGuildUser socketGuildUser)
        {
            if (_dbContext.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser();
            user.Guild = ToGuild(socketGuildUser.Guild);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            UsersAdded++;
        }

        public async Task AddUserAsync(SocketGuildUser socketGuildUser)
        {
            if (_dbContext.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser();
            user.Guild = ToGuild(socketGuildUser.Guild);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            UsersAdded++;
        }

        public void RemoveUser(SocketGuildUser user)
        {
            User userToRemove = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToRemove is null)
                return;

            _dbContext.Users.Remove(userToRemove);
            _dbContext.SaveChanges();
        }

        public async Task RemoveUserAsync(SocketGuildUser user)
        {
            User userToRemove = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userToRemove is null)
                return;

            _dbContext.Users.Remove(userToRemove);
            await _dbContext.SaveChangesAsync();
        }

        private Guild ToGuild(SocketGuild socketGuild)
        {
            return _dbContext.Guilds.FirstOrDefault(x => x.Id == socketGuild.Id) ??
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

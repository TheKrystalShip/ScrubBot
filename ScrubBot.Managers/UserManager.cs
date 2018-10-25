using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database;
using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class UserManager
    {
        private readonly SQLiteContext _dbContext;

        public UserManager(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(SocketGuildUser socketGuildUser)
        {
            if (_dbContext.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser();
            user.Guild = ToGuild(socketGuildUser.Guild);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUsersAsync(IReadOnlyCollection<SocketGuild> guilds)
        {
            List<Task> tasks = new List<Task>();

            foreach (SocketGuild guild in guilds)
            {
                foreach (SocketGuildUser user in guild.Users)
                {
                    if (user.IsBot)
                        continue;

                    tasks.Add(AddUserAsync(user));
                }
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
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

        public async Task OnUserBannedAsync(SocketUser user, SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnUserJoinedAsync(SocketGuildUser user)
        {

            await Task.CompletedTask;
        }

        public async Task OnUserLeftAsync(SocketGuildUser user)
        {

            await Task.CompletedTask;
        }

        public async Task OnUserUnbannedAsync(SocketUser user, SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task OnUserUpdatedAsync(SocketUser before, SocketUser after)
        {

            await Task.CompletedTask;
        }
    }
}

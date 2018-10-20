using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Domain;
using ScrubBot.Extensions;

using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class UserManager
    {
        private readonly Tools _tools;

        public UserManager(Tools tools)
        {
            _tools = tools;

            _tools.Client.UserBanned += UserBannedAsync;
            _tools.Client.UserJoined += UserJoinedAsync;
            _tools.Client.UserLeft += UserLeftAsync;
            _tools.Client.UserUnbanned += UserUnbannedAsync;
            _tools.Client.UserUpdated += UserUpdatedAsync;
        }

        public async Task AddUserAsync(SocketGuildUser socketGuildUser)
        {
            if (_tools.Database.Users.Any(x => x.Id == socketGuildUser.Id))
                return;

            User user = socketGuildUser.ToUser();
            user.Guild = ToGuild(socketGuildUser.Guild);

            await _tools.Database.Users.AddAsync(user);
            await _tools.Database.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(SocketGuildUser user)
        {
            User userToRemove = await _tools.Database.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userToRemove is null)
                return;

            _tools.Database.Users.Remove(userToRemove);
            await _tools.Database.SaveChangesAsync();
        }

        private Guild ToGuild(SocketGuild socketGuild)
        {
            return _tools.Database.Guilds.FirstOrDefault(x => x.Id == socketGuild.Id) ??
                   new Guild
                   {
                       Name = socketGuild.Name,
                       IconUrl = socketGuild.IconUrl,
                       Id = socketGuild.Id,
                       MemberCount = socketGuild.MemberCount
                   };
        }

        public async Task UserBannedAsync(SocketUser user, SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task UserJoinedAsync(SocketGuildUser user)
        {

            await Task.CompletedTask;
        }

        public async Task UserLeftAsync(SocketGuildUser user)
        {

            await Task.CompletedTask;
        }

        public async Task UserUnbannedAsync(SocketUser user, SocketGuild guild)
        {

            await Task.CompletedTask;
        }

        public async Task UserUpdatedAsync(SocketUser before, SocketUser after)
        {

            await Task.CompletedTask;
        }
    }
}

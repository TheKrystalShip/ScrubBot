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

            _tools.Client.UserBanned += OnUserBannedAsync;
            _tools.Client.UserJoined += OnUserJoinedAsync;
            _tools.Client.UserLeft += OnUserLeftAsync;
            _tools.Client.UserUnbanned += OnUserUnbannedAsync;
            _tools.Client.UserUpdated += OnUserUpdatedAsync;
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
            System.Console.WriteLine("User updated event called");
            await Task.CompletedTask;
        }
    }
}

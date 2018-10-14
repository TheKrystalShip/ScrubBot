using Discord.WebSocket;

using ScrubBot.Database;
using ScrubBot.Handlers;

using System.Threading.Tasks;

namespace ScrubBot.Services
{
    public class EventService : Service
    {
        private readonly DiscordSocketClient _client;
        private readonly ConversionHandler _conversionHandler;
        private readonly SQLiteContext _db;

        public EventService(DiscordSocketClient client, ConversionHandler conversionHandler, SQLiteContext dbContext)
        {
            _client = client;
            _conversionHandler = conversionHandler;
            _db = dbContext;
        }

        public async Task ChannelCreated(SocketChannel socketChannel)
        {
            await Task.CompletedTask;
        }

        public async Task ChannelUpdated(SocketChannel beforeSocketChannel, SocketChannel afterSocketChannel)
        {
            await Task.CompletedTask;
        }

        public async Task ChannelDestroyed(SocketChannel socketChannel)
        {
            await Task.CompletedTask;
        }

        public async Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
            await Task.CompletedTask;
        }

        public async Task GuildUpdated(SocketGuild before, SocketGuild after)
        {
            await Task.CompletedTask;
        }

        public async Task RoleCreated(SocketRole role)
        {
            await Task.CompletedTask;
        }

        public async Task RoleDeleted(SocketRole role)
        {
            await Task.CompletedTask;
        }

        public async Task RoleUpdated(SocketRole before, SocketRole after)
        {
            await Task.CompletedTask;
        }

        public async Task UserBanned(SocketUser user, SocketGuild guild)
        {
            await Task.CompletedTask;
        }

        public async Task UserUnbanned(SocketUser user, SocketGuild guild)
        {
            await Task.CompletedTask;
        }

        public async Task UserJoined(SocketGuildUser user)
        {
            await _conversionHandler.AddUserAsync(user);
        }

        public async Task UserLeft(SocketGuildUser user)
        {
            await _conversionHandler.RemoveUserAsync(user);
        }

        public async Task UserUpdated(SocketUser before, SocketUser after)
        {
            await Task.CompletedTask;
        }
    }
}

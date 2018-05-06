using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Handlers;

namespace ScrubBot.Services
{
    public class EventService : Service
    {
        private DiscordSocketClient _client;
        private static DatabaseContext _db;

        static EventService() => _db = new DatabaseContext();
        public EventService(DiscordSocketClient client) => _client = client;

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

        public async Task UserJoined(SocketGuildUser user) => await Task.Run(() => ConversionHandler.AddUser(user));

        public async Task UserLeft(SocketGuildUser user) => await Task.Run(() => ConversionHandler.RemoveUser(user));
        
        public async Task UserUpdated(SocketUser before, SocketUser after)
        {
            await Task.CompletedTask;
        }
    }
}
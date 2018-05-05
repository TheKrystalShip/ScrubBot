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
            
        }

        public async Task ChannelUpdated(SocketChannel beforeSocketChannel, SocketChannel afterSocketChannel)
        {

        }

        public async Task ChannelDestroyed(SocketChannel socketChannel)
        {

        }

        public async Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {

        }

        public async Task GuildUpdated(SocketGuild before, SocketGuild after)
        {

        }

        public async Task RoleCreated(SocketRole role)
        {

        }

        public async Task RoleDeleted(SocketRole role)
        {

        }

        public async Task RoleUpdated(SocketRole before, SocketRole after)
        {

        }
        
        public async Task UserBanned(SocketUser user, SocketGuild guild)
        {

        }

        public async Task UserUnbanned(SocketUser user, SocketGuild guild)
        {

        }

        public async Task UserJoined(SocketGuildUser user) => ConversionHandler.AddUser(user);

        public async Task UserLeft(SocketGuildUser user) => ConversionHandler.RemoveUser(user);
        
        public async Task UserUpdated(SocketUser before, SocketUser after)
        {

        }
    }
}
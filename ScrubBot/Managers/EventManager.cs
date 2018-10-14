using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class EventManager
    {
        private readonly DiscordSocketClient _client;
        private readonly ChannelManager _channelManager;
        private readonly GuildManager _guildManager;
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;

        public EventManager(DiscordSocketClient client, ChannelManager channelManager, GuildManager guildManager, RoleManager roleManager, UserManager userManager) : this(client)
        {
            _client = client;
            _channelManager = channelManager;
            _guildManager = guildManager;
            _roleManager = roleManager;
            _userManager = userManager;

            _client.Log += LogMessage;
            _client.Ready += Ready;

            SubscribeToClientEvents();
        }

        private Task LogMessage(LogMessage message)
        {
            if (!message.Message.Contains("OpCode"))
            {
                Console.WriteLine(message);
            }

            return Task.CompletedTask;
        }

        private async Task Ready()
        {
            try
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    foreach (SocketGuildUser user in guild.Users)
                    {
                        if (user.IsBot)
                            continue;

                        await _userManager.AddUserAsync(user).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void SubscribeToClientEvents()
        {
            _client.ChannelCreated += async (channel) => await _channelManager.ChannelCreatedAsync(channel).ConfigureAwait(false);
            _client.ChannelDestroyed += async (channel) => await _channelManager.ChannelDestroyedAsync(channel).ConfigureAwait(false);
            _client.ChannelUpdated += async (before, after) => await _channelManager.ChannelUpdatedAsync(before, after).ConfigureAwait(false);

            _client.GuildMemberUpdated += async (before, after) => await _guildManager.GuildMemberUpdatedAsync(before, after).ConfigureAwait(false);
            _client.GuildUpdated += async (before, after) => await _guildManager.GuildUpdatedAsync(before, after).ConfigureAwait(false);

            _client.RoleCreated += async (role) => await _roleManager.RoleCreatedAsync(role).ConfigureAwait(false);
            _client.RoleDeleted += async (role) => await _roleManager.RoleDeletedAsync(role).ConfigureAwait(false);
            _client.RoleUpdated += async (before, after) => await _roleManager.RoleUpdatedAsync(before, after).ConfigureAwait(false);

            _client.UserBanned += async (user, guild) => await _userManager.UserBannedAsync(user, guild).ConfigureAwait(false);
            _client.UserJoined += async (user) => await _userManager.UserJoinedAsync(user).ConfigureAwait(false);
            _client.UserLeft += async (user) => await _userManager.UserLeftAsync(user).ConfigureAwait(false);
            _client.UserUnbanned += async (user, guild) => await _userManager.UserUnbannedAsync(user, guild).ConfigureAwait(false);
            _client.UserUpdated += async (before, after) => await _userManager.UserUpdatedAsync(before, after).ConfigureAwait(false);
        }
    }
}

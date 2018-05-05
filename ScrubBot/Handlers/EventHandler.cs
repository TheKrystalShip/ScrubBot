using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ScrubBot.Services;

namespace ScrubBot.Handlers
{
    public class EventHandler
    {
        private DiscordSocketClient _client;
        private EventService _eventService;

        public EventHandler(DiscordSocketClient client) => Initialize(client);

        private void Initialize(DiscordSocketClient client)
        {
            _client = client;
            _eventService = new EventService(_client);

            _client.Log += LogMessage;
            _client.Ready += Ready;

            SubscribeToAuditService();
        }

        private async Task LogMessage(LogMessage message)
        {
            if (message.Message.Contains("OpCode")) return;

            await Task.Run(() => { Console.WriteLine(message); });
        }

        private async Task Ready() => await Task.Run(() => RegisterUsers());

        private void RegisterUsers()
        {
            //LogHandler.WriteLine(LogTarget.Console, "Stargint user registration...");

            try
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    foreach (SocketGuildUser user in guild.Users)
                    {
                        if (user.IsBot) continue;
                        
                        ConversionHandler.AddUser(user);
                    }
                }
            }
            catch (Exception e)
            {
                //LogHandler.WriteLine(LogTarget.Console, e);
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine(new LogMessage(LogSeverity.Info, "Conversion", $"Added {ConversionHandler.UsersAdded} users"));
                //LogHandler.WriteLine(LogTarget.Console, ConversionHandler.UsersAdded > 0 ? $"Done, {ConversionHandler.UsersAdded} user(s)" : "Done, no new users were added");
            }
        }

        private void SubscribeToAuditService()
        {
            _client.ChannelCreated += channel => _eventService.ChannelCreated(channel);
            _client.ChannelDestroyed += (channel) => _eventService.ChannelDestroyed(channel);
            _client.ChannelUpdated += (before, after) => _eventService.ChannelUpdated(before, after);

            _client.GuildMemberUpdated += (before, after) => _eventService.GuildMemberUpdated(before, after);
            _client.GuildUpdated += (before, after) => _eventService.GuildUpdated(before, after);

            _client.RoleCreated += (role) => _eventService.RoleCreated(role);
            _client.RoleDeleted += (role) => _eventService.RoleDeleted(role);
            _client.RoleUpdated += (before, after) => _eventService.RoleUpdated(before, after);

            _client.UserBanned += (user, guild) => _eventService.UserBanned(user, guild);
            _client.UserJoined += (user) => _eventService.UserJoined(user);
            _client.UserLeft += (user) => _eventService.UserLeft(user);
            _client.UserUnbanned += (user, guild) => _eventService.UserUnbanned(user, guild);
            _client.UserUpdated += (before, after) => _eventService.UserUpdated(before, after);
        }
    }
}
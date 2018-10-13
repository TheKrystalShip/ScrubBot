using Discord;
using Discord.WebSocket;

using ScrubBot.Services;

using System;
using System.Threading.Tasks;

namespace ScrubBot.Handlers
{
    public class EventHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly EventService _eventService;
        private readonly ConversionHandler _conversionHandler;

        public EventHandler(DiscordSocketClient client, EventService eventService, ConversionHandler conversionHandler)
        {
            _client = client;
            _eventService = eventService;
            _conversionHandler = conversionHandler;

            _client.Log += LogMessage;
            _client.Ready += () => Task.Run(Ready);

            SubscribeToAuditService();
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
                await Task.Run(() =>
                {
                    foreach (SocketGuild guild in _client.Guilds)
                    {
                        foreach (SocketGuildUser user in guild.Users)
                        {
                            if (user.IsBot) continue;

                            _conversionHandler.AddUser(user);
                        }
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine(new LogMessage(LogSeverity.Info, "Conversion", $"Added {ConversionHandler.UsersAdded} users"));
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

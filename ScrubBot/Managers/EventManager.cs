using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace ScrubBot.Managers
{
    public class EventManager
    {
        private readonly DiscordSocketClient _client;
        private readonly UserManager _userManager;

        public EventManager(DiscordSocketClient client, UserManager userManager)
        {
            _client = client;
            _userManager = userManager;

            _client.Log += LogMessage;
            _client.Ready += Ready;
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
    }
}

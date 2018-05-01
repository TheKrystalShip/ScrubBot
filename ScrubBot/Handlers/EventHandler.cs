using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ScrubBot.Handlers
{
    public class EventHandler
    {
        private DiscordSocketClient _client;

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;
            _client.Log += LogMessage;
        }

        private async Task LogMessage(LogMessage message)
        {
            if (message.Message.Contains("OpCode")) return;

            await Task.Run(() => { Console.WriteLine(message); });
        }
    }
}
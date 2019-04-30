using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using ScrubBot.Extensions;

namespace ScrubBot.Managers
{
    public class ChannelManager : IChannelManager
    {
        public ChannelManager()
        {

        }

        public async Task OnChannelCreatedAsync(SocketChannel channel)
        {
            SocketGuildChannel guildChannel = channel as SocketGuildChannel;

            if (guildChannel is null) {
                return;
            }

            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Channel: {guildChannel.Name} was created in guild: {guildChannel.Guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnChannelDestroyedAsync(SocketChannel channel)
        {
            SocketGuildChannel guildChannel = channel as SocketGuildChannel;

            if (guildChannel is null) {
                return;
            }

            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Channel: {guildChannel.Name} was destroyed in guild: {guildChannel.Guild.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnChannelUpdatedAsync(SocketChannel before, SocketChannel after)
        {
            SocketGuildChannel guildChannel = before as SocketGuildChannel;

            if (guildChannel is null) {
                return;
            }

            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Channel: {guildChannel.Name} updated"));
            Console.WriteLine(before.Compare(after).BuildString());

            await Task.CompletedTask;
        }
    }
}

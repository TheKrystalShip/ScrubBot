using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace ScrubBot.Managers
{
    public class ReactionManager : IReactionManager
    {
        public ReactionManager()
        {

        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been added to a message in {socketMessageChannel.Name}"));

            await Task.CompletedTask;
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been removed to a message in {socketMessageChannel.Name}"));

            await Task.CompletedTask;
        }
    }
}

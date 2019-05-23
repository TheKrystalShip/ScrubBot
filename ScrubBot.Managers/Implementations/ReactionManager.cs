using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Domain;
using TheKrystalShip.DependencyInjection;

namespace ScrubBot.Managers
{
    public class ReactionManager : IReactionManager
    {
        protected IDbContext Database { get; }
        protected IPrefixManager PrefixManager { get; private set; }
        protected Guild Guild { get; private set; }
        protected User User { get; private set; }

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            //Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been added to a message in {socketMessageChannel.Name}"));

            await cacheable.GetOrDownloadAsync();
            Event @event;

            if (EventExists(cacheable.Value.Id, out @event))
            {
                // Edit the message, by adding the person who added the reaction, to the list of subscribed users (also add to the Database Event)
            }
            
            await Task.CompletedTask;
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been removed to a message in {socketMessageChannel.Name}"));

            await Task.CompletedTask;
        }

        protected bool EventExists(ulong eventMessageId, out Event @event)
        {
            @event = Database.Events.FirstOrDefault(x => x.SubscribeMessageId == eventMessageId);
            return @event != null;
        }
    }
}

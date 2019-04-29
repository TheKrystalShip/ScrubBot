using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Domain;

using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Managers
{
    public class ReactionManager : IReactionManager
    {
        private readonly IDbContext _dbContext;
        private readonly ConcurrentDictionary<ulong, string> _prefixes;
        private ICommandContext _context;

        public ReactionManager(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _prefixes = new ConcurrentDictionary<ulong, string>();

            var guilds = _dbContext.Guilds.Select(x => new { x.Id, x.Prefix }).ToList();
        }

        public void SetCommandContext(ICommandContext context)
        {
            _context = context;
        }

        public Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            return Task.Run(() => Console.WriteLine($"Reaction {reaction.Emote.Name} has been added to a message in {socketMessageChannel.Name}"));
        }

        public Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            return Task.Run(() => Console.WriteLine($"Reaction {reaction.Emote.Name} has been removed to a message in {socketMessageChannel.Name}"));
        }
    }
}

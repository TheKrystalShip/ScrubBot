using System.Collections.Concurrent;
using System.Linq;

using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Database.Domain;

using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Managers
{
    public class PrefixManager : IPrefixManager
    {
        private readonly IDbContext _dbContext;
        private readonly ConcurrentDictionary<ulong, string> _prefixes;
        private ICommandContext _context;

        public PrefixManager(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _prefixes = new ConcurrentDictionary<ulong, string>();

            var guilds = _dbContext.Guilds.Select(x => new { x.Id, x.Prefix }).ToList();

            foreach (var guild in guilds)
            {
                _prefixes.TryAdd(guild.Id, guild.Prefix ?? Configuration.Get("Bot:Prefix"));
            }
        }

        public string Get()
        {
            return Get(_context.Guild.Id);
        }

        public string Get(ulong guildId)
        {
            bool hasValue = _prefixes.TryGetValue(guildId, out string value);
            return hasValue ? value : Configuration.Get("Bot:Prefix");
        }

        public bool Set(string prefix)
        {
            return Set(_context.Guild.Id, prefix);
        }

        public bool Set(ulong guildId, string prefix)
        {
            Guild guild = _dbContext.Guilds.Find(_context.Guild.Id);
            guild.Prefix = prefix;

            _dbContext.Guilds.Update(guild);
            _dbContext.SaveChanges();

            _prefixes.AddOrUpdate(guildId, prefix, (key, oldValue) => prefix);

            return true;
        }

        public void SetCommandContext(ICommandContext context)
        {
            _context = context;
        }
    }
}

using ScrubBot.Database;

using System.Collections.Concurrent;
using System.Linq;

namespace ScrubBot.Handlers
{
    public class PrefixHandler
    {
        private readonly DatabaseContext _db;

        // Thread-safe dictionaries for async operations
        private ConcurrentDictionary<ulong, string> CharPrefixDictionary { get; }
        private ConcurrentDictionary<ulong, string> StringPrefixDictionary { get; }

        public PrefixHandler(DatabaseContext dbContext)
        {
            _db = dbContext;
            CharPrefixDictionary = new ConcurrentDictionary<ulong, string>();
            StringPrefixDictionary = new ConcurrentDictionary<ulong, string>();

            var Guilds = _db.Guilds.Select(x => new { x.Id, x.CharPrefix, x.StringPrefix }).ToList();

            foreach (var guild in Guilds)
            {
                CharPrefixDictionary.TryAdd(guild.Id, guild.CharPrefix);
                StringPrefixDictionary.TryAdd(guild.Id, guild.StringPrefix);
            }
        }

        public string GetCharPrefix(ulong guildId)
        {
            bool hasValue = CharPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

        public string GetStringPrefix(ulong guildId)
        {
            bool hasValue = StringPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

        public bool SetCharPrefix(ulong guildId, string prefix)
        {
            return CharPrefixDictionary.TryAdd(guildId, prefix);
        }

        public bool SetStringPrefix(ulong guildId, string prefix)
        {
            return StringPrefixDictionary.TryAdd(guildId, prefix);
        }
    }
}
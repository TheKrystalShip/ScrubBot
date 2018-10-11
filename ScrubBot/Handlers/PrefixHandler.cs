using ScrubBot.Database;

using System.Collections.Concurrent;
using System.Linq;

namespace ScrubBot.Handlers
{
    // This entire class needs to be static, there's only gonna be one instance during the
    // lifespan of the application, no need to create instances all over the place
    // and call the database everytime.
    // This way you ensure the Prefix dictionaries always have the same values
    // for each call.
    public static class PrefixHandler
    {
        private static readonly DatabaseContext _db;

        // Thread-safe dictionaries for async operations
        private static ConcurrentDictionary<ulong, string> CharPrefixDictionary { get; }
        private static ConcurrentDictionary<ulong, string> StringPrefixDictionary { get; }

        // This constructor will only be called ONE time,
        // the first time this class is accessed.
        static PrefixHandler()
        {
            _db = new DatabaseContext();
            CharPrefixDictionary = new ConcurrentDictionary<ulong, string>();
            StringPrefixDictionary = new ConcurrentDictionary<ulong, string>();

            var Guilds = _db.Guilds.Select(x => new { Id = ulong.Parse(x.Id), x.CharPrefix, x.StringPrefix }).ToList();

            foreach (var guild in Guilds)
            {
                CharPrefixDictionary.TryAdd(guild.Id, guild.CharPrefix);
                StringPrefixDictionary.TryAdd(guild.Id, guild.StringPrefix);
            }
        }

        public static string GetCharPrefix(ulong guildId)
        {
            bool hasValue = CharPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

        public static string GetStringPrefix(ulong guildId)
        {
            bool hasValue = StringPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

        public static bool SetCharPrefix(string guildId, string prefix)
        {
            return CharPrefixDictionary.TryAdd(ulong.Parse(guildId), prefix);
        }

        public static bool SetStringPrefix(string guildId, string prefix)
        {
            return StringPrefixDictionary.TryAdd(ulong.Parse(guildId), prefix);
        }
    }
}
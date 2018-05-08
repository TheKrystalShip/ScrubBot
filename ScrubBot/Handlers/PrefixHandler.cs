using System.Collections.Generic;
using System.Linq;
using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Properties;

namespace ScrubBot.Handlers
{
    class PrefixHandler
    {
        private DatabaseContext db;
        private static Dictionary<string, string> CharPrefixDictionary { get; set; } = new Dictionary<string, string>();
        private static Dictionary<string, string> StringPrefixDictionary { get; set; } = new Dictionary<string, string>();

        public PrefixHandler() => Initialize();

        private void Initialize()
        {
            db = new DatabaseContext();
            List<Guild> Guilds = db.Guilds.ToList();
            
            foreach (Guild guild in Guilds)
            {
                CharPrefixDictionary.Add(guild.Id, guild.CharPrefix ?? Resources.DefaultCharPrefix);
                StringPrefixDictionary.Add(guild.Id, guild.StringPrefix ?? Resources.DefaultCharPrefix);
            }
        }

        public static bool GetCharPrefix(string guildId, out string value) => CharPrefixDictionary.TryGetValue(guildId, out value);
        
        public static bool GetStringPrefix(string guildId, out string value) => StringPrefixDictionary.TryGetValue(guildId, out value);
        
        public static void SetCharPrefix(string guildId, string prefix) => CharPrefixDictionary[guildId] = prefix;

        public static void SetStringPrefix(string guildId, string prefix) => StringPrefixDictionary[guildId] = prefix;

        public void Dispose()
        {
            db = null;
            CharPrefixDictionary = null;
            StringPrefixDictionary = null;
        }
    }
}
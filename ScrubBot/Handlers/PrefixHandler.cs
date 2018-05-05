using System;
using System.Collections.Generic;
using System.Linq;

using ScrubBot.Database;
using ScrubBot.Database.Models;

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
                CharPrefixDictionary.Add(guild.Id, guild.CharPrefix);
                StringPrefixDictionary.Add(guild.Id, guild.StringPrefix);
            }
        }

        public static string GetCharPrefix(string guildId)
        {
            bool hasValue = CharPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

        public static string GetStringPrefix(string guildId)
        {
            bool hasValue = StringPrefixDictionary.TryGetValue(guildId, out string value);
            return value;
        }

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
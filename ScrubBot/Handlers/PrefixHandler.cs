using System;
using System.Collections.Generic;
using System.Linq;

using ScrubBot.Data;

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
            List<Guild> Guilds = new List<Guild>();
            db = new DatabaseContext();
            try
            {
                Guilds = db.Guilds.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (Guild guild in Guilds)
            {
                CharPrefixDictionary.Add(guild.Id, guild.CharPrefix);
                StringPrefixDictionary.Add(guild.Id, guild.StringPrefix);
            }
        }

        public static string GetCharPrefix(string guildId)
        {
            bool hasValue = CharPrefixDictionary.TryGetValue(guildId, out string value);
            return value ?? "#";
        }

        public static string GetStringPrefix(string guildId)
        {
            bool hasValue = StringPrefixDictionary.TryGetValue(guildId, out string value);
            return value ?? "ScrubBot, ";
        }

        public static void SetCharPrefix(string guildId, char prefix) => CharPrefixDictionary[guildId] = prefix.ToString();

        public static void SetStringPrefix(string guildId, string prefix) => StringPrefixDictionary[guildId] = prefix;

        public void Dispose()
        {
            db = null;
            CharPrefixDictionary = null;
            StringPrefixDictionary = null;
        }
    }
}
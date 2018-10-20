using Discord.Commands;
using Discord.WebSocket;

using ScrubBot.Database;
using ScrubBot.Handlers;

namespace ScrubBot
{
    public class Tools
    {
        public DiscordSocketClient Client { get; private set; }
        public SQLiteContext Database { get; private set; }
        public CommandService CommandService { get; private set; }
        public PrefixHandler Prefix { get; private set; }

        public Tools(DiscordSocketClient client, SQLiteContext database, CommandService commandService, PrefixHandler prefix)
        {
            Client = client;
            Database = database;
            CommandService = commandService;
            Prefix = prefix;
        }
    }
}

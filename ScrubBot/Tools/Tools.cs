using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Handlers;

namespace ScrubBot
{
    public class Tools
    {
        public SQLiteContext Database { get; private set; }
        public CommandService CommandService { get; private set; }
        public PrefixHandler Prefix { get; private set; }

        public Tools(SQLiteContext database, CommandService commandService, PrefixHandler prefix)
        {
            Database = database;
            CommandService = commandService;
            Prefix = prefix;
        }
    }
}

using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Handlers;

namespace ScrubBot
{
    public class Tools
    {
        public DatabaseContext Database { get; set; }
        public CommandService CommandService { get; set; }
        public PrefixHandler Prefix { get; set; }

        public Tools(DatabaseContext database, CommandService commandService, PrefixHandler prefix)
        {
            Database = database;
            CommandService = commandService;
            Prefix = prefix;
        }
    }
}

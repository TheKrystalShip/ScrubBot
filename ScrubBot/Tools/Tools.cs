using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Database.Models;
using ScrubBot.Handlers;
using ScrubBot.Managers;

namespace ScrubBot
{
    public class Tools
    {
        public IManager<User> UserManager { get; set; }
        public DatabaseContext Database { get; set; }
        public CommandService CommandService { get; set; }
        public PrefixHandler Prefix { get; set; }

        public Tools(IManager<User> userManager, DatabaseContext databaseContext, CommandService commandService, PrefixHandler prefixHandler)
        {
            UserManager = userManager;
            Database = databaseContext;
            CommandService = commandService;
            Prefix = prefixHandler;
        }
    }
}

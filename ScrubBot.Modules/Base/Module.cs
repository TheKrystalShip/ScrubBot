using Discord;
using Discord.Commands;

using ScrubBot.Database;
using ScrubBot.Domain;
using ScrubBot.Managers;
using ScrubBot.Tools;

using System;
using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    public abstract class Module : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; private set; }
        public SQLiteContext Database { get; private set; }
        public PrefixManager Prefix { get; private set; }
        public Guild Guild { get; protected set; }
        public User User { get; protected set; }

        public Module()
        {
            CommandService = Container.Get<CommandService>();
            Database = Container.Get<SQLiteContext>();
            Prefix = Container.Get<PrefixManager>();
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);

            Guild = Database.Guilds.Find(Context.Guild?.Id);
            User = Database.Users.Find(Context.User?.Id);

            if (Guild is null)
            {
                Console.WriteLine(new LogMessage(LogSeverity.Warning, GetType().Name, "Guild is null in current scope"));
            }

            if (User is null)
            {
                Console.WriteLine(new LogMessage(LogSeverity.Warning, GetType().Name, "User is null in current scope"));
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);

            Database.Guilds.Update(Guild);
            Database.Users.Update(User);
            Database.SaveChanges();

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Handle exceptions in here
        }

        protected virtual async Task<IUserMessage> ReplyAsync(EmbedBuilder embedBuilder)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build(), null).ConfigureAwait(false);
        }
    }
}

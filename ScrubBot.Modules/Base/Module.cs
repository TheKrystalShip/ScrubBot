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
        public IPrefixManager Prefix { get; private set; }
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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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
            Exception exception = e.ExceptionObject as Exception;

            if (exception is null)
                return;

            if (exception.InnerException != null)
            {
                Console.WriteLine(exception.InnerException);
            }

            Console.WriteLine(exception);
        }

        protected virtual async Task<IUserMessage> ReplyAsync(EmbedBuilder embedBuilder)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build(), null).ConfigureAwait(false);
        }
    }
}

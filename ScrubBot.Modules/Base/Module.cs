using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Rest;

using ScrubBot.Database;
using ScrubBot.Database.Domain;
using ScrubBot.Managers;

using TheKrystalShip.DependencyInjection;

namespace ScrubBot.Modules
{
    public class Module : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; private set; }
        public IDbContext Database { get; private set; }
        public IPrefixManager Prefix { get; private set; }
        public Guild Guild { get; protected set; }
        public User User { get; protected set; }

        public Module()
        {
            //CommandService = Container.Get<CommandService>();
            Database = Container.Get<IDbContext>();
            Prefix = Container.Get<IPrefixManager>();
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

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

            if (Guild != null)
            {
                Database.Guilds.Update(Guild);
            }

            if (User != null)
            {
                Database.Users.Update(User);
            }

            Database.SaveChanges();

            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
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

        protected Task<RestUserMessage> ReplyAsync(EmbedBuilder embedBuilder)
        {
            return Context.Channel.SendMessageAsync(text: string.Empty, isTTS: false, embed: embedBuilder.Build(), options: null);
        }

        protected virtual Task<RestUserMessage> ReplyAsync(Embed embed)
        {
            return Context.Channel.SendMessageAsync(text: string.Empty, isTTS: false, embed: embed, options: null);
        }
    }
}

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
        protected IDbContext Database { get; private set; }
        protected IPrefixManager PrefixManager { get; private set; }
        protected Guild Guild { get; private set; }
        protected User User { get; private set; }

        protected string Prefix
        {
            get { return PrefixManager.Get(Context.Guild.Id); }
            set { PrefixManager.Set(Context.Guild.Id, value); }
        }

        public Module()
        {
            Database = Container.Get<IDbContext>();
            PrefixManager = Container.Get<IPrefixManager>();
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Guild = Database.Guilds.Find(Context.Guild?.Id);
            User = Database.Users.Find(Context.User?.Id);

            PrefixManager.SetCommandContext(Context);

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
            if (!(e.ExceptionObject is Exception exception))
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

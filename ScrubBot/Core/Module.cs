using System.Threading.Tasks;
using Discord;
using Discord.Commands;

using ScrubBot.Database.Models;

namespace ScrubBot
{
    public abstract class Module : ModuleBase<SocketCommandContext>
    {
        public Tools Tools { get; private set; }
        public Guild Guild { get; protected set; }
        public User User { get; protected set; }

        public Module(Tools tools)
        {
            Tools = tools;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            base.BeforeExecute(command);
            Guild = Tools.Database.Guilds.Find(Context.Guild?.Id);
            User = Tools.Database.Users.Find(Context.User?.Id);
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            Tools.Database.Guilds.Update(Guild);
            Tools.Database.Users.Update(User);
            Tools.Database.SaveChanges();
        }

        protected virtual async Task<IUserMessage> ReplyAsync(EmbedBuilder embedBuilder)
        {
            return await Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build(), null).ConfigureAwait(false);
        }
    }
}

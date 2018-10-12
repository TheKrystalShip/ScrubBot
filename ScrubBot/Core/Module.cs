using Discord.Commands;

using ScrubBot.Database.Models;

using System;

namespace ScrubBot
{
    public abstract class Module : ModuleBase<SocketCommandContext>, IDisposable
    {
        public Tools Tools { get; private set; }
        public Guild Guild { get; protected set; }
        public User User { get; protected set; }

        public Module(Tools tools)
        {
            Tools = tools;
        }

        public void Dispose()
        {
            Tools.Database.Guilds.Update(Guild);
            Tools.Database.Users.Update(User);
            Tools.Database.SaveChanges();
        }
    }
}

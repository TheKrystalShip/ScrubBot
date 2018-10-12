using Discord.Commands;

namespace ScrubBot
{
    public abstract class Module : ModuleBase<SocketCommandContext>
    {
        public Tools Tools { get; private set; }

        protected Module(Tools tools)
        {
            Tools = tools;
        }
    }
}

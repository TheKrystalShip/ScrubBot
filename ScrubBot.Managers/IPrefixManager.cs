using Discord.Commands;

namespace ScrubBot.Managers
{
    public interface IPrefixManager
    {
        string Get();
        string Get(ulong guildId);
        bool Set(string prefix);
        bool Set(ulong guildId, string prefix);
        void SetCommandContext(ICommandContext context);
    }
}

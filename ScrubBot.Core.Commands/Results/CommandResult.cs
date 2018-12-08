using Discord;
using Discord.Commands;

namespace ScrubBot.Core.Commands
{
    public class CommandResult : RuntimeResult
    {
        public Embed Embed { get; protected set; }

        public CommandResult(CommandError? error, string reason) : base(error, reason)
        {

        }
    }
}

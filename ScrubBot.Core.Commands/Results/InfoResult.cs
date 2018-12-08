using Discord;
using Discord.Commands;

using ScrubBot.Extensions;

namespace ScrubBot.Core.Commands
{
    public class InfoResult : CommandResult
    {
        // Redirect all constructors to this one
        public InfoResult(CommandError? error, string reason) : base(error, reason)
        {
            Embed = new EmbedBuilder().CreateMessage("Info", reason).Build();
        }

        public InfoResult(string message) : this(null, message)
        {

        }

        public InfoResult() : this(null, "Info")
        {

        }
    }
}

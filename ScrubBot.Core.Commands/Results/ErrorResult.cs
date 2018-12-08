using Discord;
using Discord.Commands;

using ScrubBot.Extensions;

namespace ScrubBot.Core.Commands
{
    public class ErrorResult : CommandResult
    {
        // Redirect all constructors to this one
        public ErrorResult(CommandError? error, string reason) : base(error, reason)
        {
            Embed = new EmbedBuilder().CreateError(reason).Build();
        }

        public ErrorResult(string message) : this(CommandError.Unsuccessful, message)
        {

        }

        public ErrorResult() : this(null, "Error")
        {

        }        
    }
}

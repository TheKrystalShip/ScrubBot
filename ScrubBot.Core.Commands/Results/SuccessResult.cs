using Discord;
using Discord.Commands;

using ScrubBot.Extensions;

namespace ScrubBot.Core.Commands
{
    public class SuccessResult : CommandResult
    {
        // Redirect all constructors to this one
        public SuccessResult(CommandError? error, string reason) : base(error, reason)
        {
            Embed = new EmbedBuilder().CreateSuccess(reason).Build();
        }

        public SuccessResult(string message) : this(null, message)
        {

        }

        public SuccessResult() : this(null, "Success")
        {

        }        
    }
}

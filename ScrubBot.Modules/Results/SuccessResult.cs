using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class SuccessResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public SuccessResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public SuccessResult(string message) : this(null, message)
        {

        }

        public SuccessResult() : this(null, "Success")
        {

        }

        public SuccessResult(EmbedBuilder embedBuilder) : this(null, "Success")
        {
            Embed = embedBuilder.Build();
        }

        public SuccessResult(Embed embed) : this(null, "Success")
        {
            Embed = embed;
        }
    }
}

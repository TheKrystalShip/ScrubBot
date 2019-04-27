using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class WarningResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public WarningResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public WarningResult(string message) : this(null, message)
        {

        }

        public WarningResult() : this(null, "Warning")
        {

        }

        public WarningResult(EmbedBuilder embedBuilder) : this(null, "Warning")
        {
            Embed = embedBuilder.Build();
        }

        public WarningResult(Embed embed) : this(null, "Warning")
        {
            Embed = embed;
        }
    }
}

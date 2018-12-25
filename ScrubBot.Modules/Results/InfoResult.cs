using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class InfoResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public InfoResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public InfoResult(string message) : this(null, message)
        {

        }

        public InfoResult() : this(null, "Info")
        {

        }

        public InfoResult(EmbedBuilder embedBuilder) : this(null, "Info")
        {
            Embed = embedBuilder.Build();
        }

        public InfoResult(Embed embed) : this(null, "Warning")
        {
            Embed = embed;
        }
    }
}

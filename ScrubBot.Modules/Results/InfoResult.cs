using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class InfoResult : RuntimeResult // TODO: Add Color.Purple as standard error embeds
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

        public InfoResult(EmbedBuilder embedBuilder) : this(null, string.Empty)
        {
            Embed = embedBuilder.Build();
        }

        public InfoResult(Embed embed) : this(null, string.Empty)
        {
            Embed = embed;
        }
    }
}

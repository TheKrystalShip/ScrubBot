using Discord;
using Discord.Commands;

using ScrubBot.Extensions;
using ScrubBot.Tools;

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
            Embed = EmbedFactory.Create(x =>
            {
                x.CreateInfo(message);
            });
        }

        public InfoResult() : this(null, "Info")
        {

        }

        public InfoResult(EmbedBuilder embedBuilder) : this(null, "Info")
        {
            Embed = embedBuilder.Build();
        }

        public InfoResult(Embed embed) : this(null, "Info")
        {
            Embed = embed;
        }
    }
}

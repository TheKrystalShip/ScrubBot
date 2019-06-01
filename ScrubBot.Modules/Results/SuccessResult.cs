using Discord;
using Discord.Commands;
using ScrubBot.Tools;

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
            Embed = EmbedFactory.Create(x =>
            {
                x.WithTitle("Error");
                x.WithColor(Color.Red);
                x.WithDescription(message);
            });
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

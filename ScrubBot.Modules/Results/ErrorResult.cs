using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class ErrorResult : RuntimeResult // TODO: Add Color.Red as standard error embeds
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public ErrorResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public ErrorResult(string message) : this(CommandError.Unsuccessful, message)
        {

        }

        public ErrorResult() : this(null, "Error")
        {

        }

        public ErrorResult(Embed embed) : this(CommandError.Unsuccessful, string.Empty)
        {
            Embed = embed;
        }

        public ErrorResult(EmbedBuilder embedBuilder) : this(CommandError.Unsuccessful, "Error")
        {
            Embed = embedBuilder.Build();
        }

        public ErrorResult(string message, EmbedBuilder embedBuilder) : this(CommandError.Unsuccessful, message)
        {
            Embed = embedBuilder.Build();
        }

        public ErrorResult(CommandError? error, EmbedBuilder embedBuilder) : this(error, "Error")
        {
            Embed = embedBuilder.Build();
        }

        public ErrorResult(CommandError? error, string message, EmbedBuilder embedBuilder) : this(error, message)
        {
            Embed = embedBuilder.Build();
        }

        public ErrorResult(string message, Embed embed) : this(CommandError.Unsuccessful, message)
        {
            Embed = embed;
        }

        public ErrorResult(CommandError? error, Embed embed) : this(error, "Error")
        {
            Embed = embed;
        }

        public ErrorResult(CommandError? error, string message, Embed embed) : this(error, message)
        {
            Embed = embed;
        }
    }
}

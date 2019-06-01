using Discord;
using Discord.Commands;

namespace ScrubBot.Modules
{
    public class EmptyResult : RuntimeResult
    {
        public Embed Embed { get; }

        public EmptyResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public EmptyResult(string message) : this(null, message)
        {

        }

        public EmptyResult() : this(null, null)
        {

        }

        public EmptyResult(EmbedBuilder embedBuilder) : this(null, "Success")
        {
            Embed = embedBuilder.Build();
        }

        public EmptyResult(Embed embed) : this(null, "Success")
        {
            Embed = embed;
        }
    }
}

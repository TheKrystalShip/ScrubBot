using Discord.Commands;

namespace ScrubBot.Modules
{
    public class EmptyResult : RuntimeResult
    {
        public EmptyResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public EmptyResult() : this(null, null)
        {

        }
    }
}

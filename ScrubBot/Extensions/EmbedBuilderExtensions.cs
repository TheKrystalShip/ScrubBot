using Discord;

namespace ScrubBot.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder CreateError(this EmbedBuilder embedBuilder, string errorMessage)
        {
            return embedBuilder
                .WithColor(Color.Red)
                .WithTitle("Error")
                .WithDescription(errorMessage);
        }

        public static EmbedBuilder CreateSuccess(this EmbedBuilder embedBuilder)
        {
            return embedBuilder
                .WithColor(Color.Green)
                .WithTitle("Success");
        }
    }
}

using System;

using Discord;

namespace ScrubBot.Tools
{
    public static class EmbedFactory
    {
        public static Embed Create(Action<EmbedBuilder> action)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();
            action(embedBuilder);

            return embedBuilder.Build();
        }
    }
}

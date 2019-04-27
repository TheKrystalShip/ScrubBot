using System.Collections.Generic;

using Discord;

using ScrubBot.Database.Domain;

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

        public static EmbedBuilder CreateSuccess(this EmbedBuilder embedBuilder, string message)
        {
            return embedBuilder
                .WithColor(Color.Green)
                .WithTitle("Success")
                .WithDescription(message);
        }

        public static EmbedBuilder CreateMessage(this EmbedBuilder embedBuilder, string title, string message)
        {
            return embedBuilder
                .WithColor(Color.Purple)
                .WithTitle(title)
                .WithDescription(message);
        }

        public static EmbedBuilder CreateEventEmbed(this EmbedBuilder embedBuilder, string title, List<Event> events)
        {
            embedBuilder
                .WithColor(Color.Purple)
                .WithTitle(title);

            foreach (Event @event in events)
            {
                embedBuilder.AddField(@event.Title, $"Occurence date: {@event.OccurenceDate:dd-MM-yyyy HH:MM}\nDescription: {@event.Description}\nSubscribers: {@event.Subscribers.Count}/{@event.MaxSubscribers}");
            }

            return embedBuilder;
        }
    }
}

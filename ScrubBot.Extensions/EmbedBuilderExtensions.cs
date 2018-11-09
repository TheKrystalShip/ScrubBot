using Discord;

using ScrubBot.Domain;

using System.Collections.Generic;

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

            foreach (var _event in events)
            {
                embedBuilder.AddField(_event.Title,
                    $"Occurence date: {_event.OccurenceDate:dd-MM-yyyy HH:MM}\nDescription: {_event.Description}\nSubscribers: {_event.Subscribers.Count}/{_event.MaxSubscribers}");
            }

            return embedBuilder;
        }
    }
}

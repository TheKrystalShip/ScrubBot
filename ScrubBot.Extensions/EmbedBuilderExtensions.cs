using System.Collections.Generic;

using Discord;
using Discord.WebSocket;

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
                embedBuilder.AddField(@event.Title, $"Occurence date: {@event.OccurenceDate:f}\nDescription: {@event.Description}\nSubscribers: {@event.Subscribers.Count}/{@event.MaxSubscribers}");

            return embedBuilder;
        }

        public static EmbedBuilder CreateListEventEmbed(this EmbedBuilder embedBuilder, Event @event, ISocketMessageChannel socketMessageChannel)
        {
            embedBuilder.Title = @event.Title;
            embedBuilder.Description = @event.Description;
            embedBuilder.WithColor(Color.Orange);
            embedBuilder.AddField("Occurence date", @event.OccurenceDate.ToString("f"));

            string participants = $"1. {socketMessageChannel.GetUserAsync(@event.Author.Id).Result.Mention} (Author)";

            for (int index = 0; index < @event.Subscribers.Count; index++)
                participants += $"\n{index + 2} {socketMessageChannel.GetUserAsync(@event.Subscribers[index].Id).Result.Mention}";

            embedBuilder.AddField("Participants", participants);
            return embedBuilder;
        }
    }
}

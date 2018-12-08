using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore.Internal;

using ScrubBot.Core.Commands;
using ScrubBot.Database.Domain;
using ScrubBot.Extensions;

namespace ScrubBot.Modules
{
    public class EventModule : Module
    {
        public EventModule()
        {

        }

        [Command("ShowEvents"), Summary("Show the first 10 upcoming events")]
        public async Task<RuntimeResult> ShowEvents(int amount = 10)
        {
            List<Event> events = Database.Events
                .Where(x => x.Guild.Id == Guild.Id)
                .OrderByDescending(x => x.CreationDate)
                .Take(amount)
                .ToList();

            if (events.Count is 0)
            {
                return Result.Error("There are no events to be displayed!");
            }

            await ReplyAsync(new EmbedBuilder().CreateEventEmbed("Upcoming events", events));
            return Result.Success("Ok");
        }

        // @ScrubBot CreateEvent "Title goes here" "Description goes here" 24/10/2018 20
        [Command("CreateEvent"), Summary("Create a new event")]
        public async Task<RuntimeResult> CreateEvent(string eventTitle, string description, DateTime occurenceDateTimeUTC, int maxSubscribers)
        {
            if (Database.Events.Any(x => x.Title == eventTitle && x.Guild.Id == Guild.Id))
            {
                return Result.Error($"An event with the name **{eventTitle}** already exists!");
            }

            Event newEvent = new Event
            {
                Title = eventTitle,
                Description = description,
                Guild = Guild, 
                OccurenceDate = occurenceDateTimeUTC.ToUniversalTime(),
                Author = User,
                MaxSubscribers = maxSubscribers
            };

            await Database.Events.AddAsync(newEvent);

            return Result.Success($"Event **{eventTitle}** has been successfully created for **{occurenceDateTimeUTC}** with a max of **{maxSubscribers}** subscribers!");
        }

        [Command("JoinEvent"), Summary("Join a specific event")]
        public async Task<RuntimeResult> JoinEvent(string eventTitle)
        {            
            Event @event = Database.Events.FirstOrDefault(x => x.Title == eventTitle && x.Guild.Id == Guild.Id);

            if (@event is null)
            {
                return Result.Error(CommandError.ObjectNotFound, "No event with that title was found");
            }

            if (@event.Author.Id == User.Id)
            {
                return Result.Error($"**{User.Username}** cannot subscribe to their own event!");
            }

            if (@event.Subscribers.Any(x => x.Id == User.Id))
            {
                return Result.Error($"**{User.Username}** is already subscribed to **{eventTitle}**!");
            }

            if (@event.Subscribers.Count == @event.MaxSubscribers)
            {
                return Result.Error($"Event **{eventTitle}** is already full!");
            }

            @event.Subscribers.Add(User);
            Database.Events.Update(@event);

            return Result.Success($"**{User.Username}** has successfully joined event **{eventTitle}** ({@event.Subscribers.Count}/{@event.MaxSubscribers})");
        }

        [Command("DeleteEvent"), Summary("Delete one of your events")]
        public async Task<RuntimeResult> DeleteEvent(string eventTitle)
        {
            Event @event = Database.Events.FirstOrDefault(x => x.Guild.Id == Guild.Id && x.Title == eventTitle);

            if (@event is null)
            {
                return Result.Error($"Unable to find event **{eventTitle}**!");
            }

            if (@event.Author.Id != User.Id || !Context.Guild.GetUser(User.Id).GuildPermissions.Administrator)
            {
                return Result.Error("You are not allowed to modify someone else's event");
            }

            Database.Events.Remove(@event);

            return Result.Success($"Successfully deleted event **{@event.Title}**!");
        }
    }
}

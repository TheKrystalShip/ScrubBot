using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using ScrubBot.Database.Domain;
using ScrubBot.Extensions;

namespace ScrubBot.Modules
{
    public class EventModule : Module
    {
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
                return new ErrorResult("There are no events to be displayed!");
            }

            await ReplyAsync(new EmbedBuilder().CreateEventEmbed("Upcoming events", events));
            return new EmptyResult();
        }

        // @ScrubBot CreateEvent "Title goes here" "Description goes here" 24/10/2018 20
        [Command("CreateEvent"), Summary("Create a new event")]
        public async Task<RuntimeResult> CreateEvent(string eventTitle, string description, DateTime occurenceDateTimeUTC, int maxSubscribers)
        {
            if (Database.Events.Any(x => x.Title == eventTitle && x.Guild.Id == Guild.Id))
            {
                return new ErrorResult($"An event with the name **{eventTitle}** already exists!");
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

            return new SuccessResult($"Event **{eventTitle}** has been successfully created for **{occurenceDateTimeUTC}** with a max of **{maxSubscribers}** subscribers!");
        }

        [Command("JoinEvent"), Summary("Join a specific event")]
        public async Task<RuntimeResult> JoinEvent(string eventTitle)
        {            
            Event @event = Database.Events.FirstOrDefault(x => x.Title == eventTitle && x.Guild.Id == Guild.Id);

            if (@event is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No event with that title was found");
            }

            if (@event.Author.Id == User.Id)
            {
                return new ErrorResult($"**{User.Username}** cannot subscribe to their own event!");
            }

            if (@event.Subscribers.Any(x => x.Id == User.Id))
            {
                return new ErrorResult($"**{User.Username}** is already subscribed to **{eventTitle}**!");
            }

            if (@event.Subscribers.Count == @event.MaxSubscribers)
            {
                return new ErrorResult($"Event **{eventTitle}** is already full!");
            }

            @event.Subscribers.Add(User);
            Database.Events.Update(@event);

            return new SuccessResult($"**{User.Username}** has successfully joined event **{eventTitle}** ({@event.Subscribers.Count}/{@event.MaxSubscribers})");
        }

        [Command("DeleteEvent"), Summary("Delete one of your events")]
        public async Task<RuntimeResult> DeleteEvent(string eventTitle)
        {
            Event @event = Database.Events.FirstOrDefault(x => x.Guild.Id == Guild.Id && x.Title == eventTitle);

            if (@event is null)
            {
                return new ErrorResult($"Unable to find event **{eventTitle}**!");
            }

            if (@event.Author.Id != User.Id || !Context.Guild.GetUser(User.Id).GuildPermissions.Administrator)
            {
                return new ErrorResult("You are not allowed to modify someone else's event");
            }

            Database.Events.Remove(@event);

            return new SuccessResult($"Successfully deleted event **{@event.Title}**!");
        }
    }
}

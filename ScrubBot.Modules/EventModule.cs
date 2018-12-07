using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore.Internal;

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
        public async Task ShowEvents(int amount = 10)
        {
            List<Event> events = Database.Events
                .Where(x => x.Guild.Id == Guild.Id)
                .OrderByDescending(x => x.CreationDate)
                .Take(amount)
                .ToList();

            if (events.Count is 0)
            {
                await ReplyAsync(new EmbedBuilder().CreateError("There are no events to be displayed!"));
                return;
            }

            await ReplyAsync(new EmbedBuilder().CreateEventEmbed("Upcoming events", events));
        }

        // @ScrubBot CreateEvent "Title goes here" "Description goes here" 24/10/2018 20
        [Command("CreateEvent"), Summary("Create a new event")]
        public async Task CreateEvent(string eventTitle, string description, DateTime occurenceDateTimeUTC, int maxSubscribers)
        {
            if (Database.Events.Any(x => x.Title == eventTitle && x.Guild.Id == Guild.Id))
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"An event with the name **{eventTitle}** already exists!"));
                return;
            }

            Event newEvent = new Event
            {
                Title = eventTitle,
                Description = description,
                Guild = base.Guild, 
                OccurenceDate = occurenceDateTimeUTC.ToUniversalTime(),
                Author = base.User,
                MaxSubscribers = maxSubscribers
            };

            await Database.Events.AddAsync(newEvent);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Event **{eventTitle}** has been successfully created for **{occurenceDateTimeUTC}** with a max of **{maxSubscribers}** subscribers!"));
        }

        [Command("JoinEvent"), Summary("Join a specific event")]
        public async Task JoinEvent(string eventTitle)
        {
            bool hasEvent = Database.Events.Any(x => x.Title == eventTitle);
            bool isSameServer = Database.Events.Any(x => x.Guild.Id == Guild.Id);
            string username = User.Username;

            if (!hasEvent || !isSameServer)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"Could not find an event with eventTitle **{eventTitle}** for this server!"));
                return;
            }
            
            Event _event = Database.Events.First(x => x.Title == eventTitle && x.Guild.Id == Guild.Id);

            if (_event.Author.Id == User.Id)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"**{username}** cannot subscribe to their own event!"));
                return;
            }

            if (_event.Subscribers.Any(x => x.Id == User.Id))
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"**{username}** is already subscribed to **{eventTitle}**!"));
                return;
            }

            if (_event.Subscribers.Count == _event.MaxSubscribers)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"Event **{eventTitle}** is already full!"));
                return;
            }

            _event.Subscribers.Add(User);
            await ReplyAsync(new EmbedBuilder().CreateSuccess($"**{username}** has successfully joined event **{eventTitle}** ({_event.Subscribers.Count}/{_event.MaxSubscribers})"));
        }

        [Command("DeleteEvent"), Summary("Delete one of your events")]
        public async Task DeleteEvent(string eventTitle)
        {
            Event _event = Database.Events.FirstOrDefault(x => x.Guild.Id == Guild.Id && x.Title == eventTitle);

            if (_event is null)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"Unable to find event **{eventTitle}**!"));
                return;
            }

            if (_event.Author.Id != User.Id || !Context.Guild.GetUser(User.Id).GuildPermissions.Administrator)
            {
                await ReplyAsync(new EmbedBuilder().CreateError("You are not allowed to modify someone else's event"));
                return;
            }

            Database.Events.Remove(_event);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Successfully deleted event **{_event.Title}**!"));
        }
    }
}

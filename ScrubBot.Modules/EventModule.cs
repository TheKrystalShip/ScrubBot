using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore.Internal;

using ScrubBot.Domain;
using ScrubBot.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrubBot.Modules
{
    public class EventModule : Module
    {
        public EventModule()
        {

        }

        [Command("ShowEvents")]
        public async Task ShowEvents(int amount = 10)
        {
            List<Event> events = Database.Events
                .Where(x => x.Guild.Id == Context.Guild.Id)
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
        [Command("CreateEvent")]
        public async Task CreateEvent(string title, string description, DateTime occurenceDateTime, int maxSubscribers)
        {
            if (Database.Events.Any(x => x.Title == title && x.Guild.Id == Context.Guild.Id))
            {
                await ReplyAsync(new EmbedBuilder().CreateError("An event with this name already exists!"));
                return;
            }

            Event newEvent = new Event
            {
                Title = title,
                Description = description,
                Guild = base.Guild, 
                OccurenceDate = occurenceDateTime.ToUniversalTime(),
                Author = base.User,
                MaxSubscribers = maxSubscribers
            };

            await Database.Events.AddAsync(newEvent);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Event {title} has been successfully added for {occurenceDateTime} with a max of {maxSubscribers} subscribers!"));
        }

        [Command("DeleteEvent")]
        public async Task DeleteEvent(string title)
        {
            Event _event = Database.Events.FirstOrDefault(x => x.Guild.Id == Context.Guild.Id && x.Title == title);

            if (_event is null)
            {
                await ReplyAsync(new EmbedBuilder().CreateError($"Unable to find event {title}!"));
                return;
            }

            if (_event.Author.Id != Context.User.Id || !Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator)
            {
                await ReplyAsync(new EmbedBuilder().CreateError("You are not allowed to modify someone else's event"));
                return;
            }

            Database.Events.Remove(_event);

            await ReplyAsync(new EmbedBuilder().CreateSuccess($"Successfully removed event {_event.Title}!"));
        }
    }
}

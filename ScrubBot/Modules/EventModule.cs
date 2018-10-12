using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Internal;
using ScrubBot.Database.Models;
using ScrubBot.Extensions;

namespace ScrubBot.Modules
{
    public class EventModule : Module
    {
        public EventModule(Tools tools) : base(tools)
        {
            Guild = Tools.Database.Guilds.Find(Context.Guild.Id);
            User = Tools.Database.Users.Find(Context.User.Id);
        }

        [Command("ShowEvents")]
        public async Task ShowEvents(int amount = 10)
        {
            EmbedBuilder embedBuilder;

            List<Event> events = Tools.Database.Events
                .Where(x => x.Guild.Id == Context.Guild.Id)
                .OrderByDescending(x => x.CreationDate)
                .Take(amount)
                .ToList();

            if (events.Count is 0)
            {
                embedBuilder = new EmbedBuilder().CreateError("There are no events to be displayed!");
                await ReplyAsync(string.Empty, false, embedBuilder.Build());
                return;
            }

            embedBuilder = new EmbedBuilder().CreateEventEmbed("Upcoming events", events);
            await ReplyAsync(string.Empty, false, embedBuilder.Build());
        }

        // @ScrubBot CreateEvent "Title goes here" "Description goes here" 24/10/2018 20
        [Command("CreateEvent")]
        public async Task CreateEvent(string title, string description, DateTime occurenceDateTime, int maxSubscribers)
        {
            EmbedBuilder embedBuilder;

            if (Tools.Database.Events.Any(x => x.Title == title && x.Guild.Id == Context.Guild.Id))
            {
                embedBuilder = new EmbedBuilder().CreateError("An event with this name already exists!");
                await ReplyAsync(string.Empty, false, embedBuilder.Build());
                return;
            }
            
            Event newEvent = new Event
            {
                Title = title,
                Description = description,
                OccurenceDate = occurenceDateTime,
                Author = (Context.User as SocketGuildUser)?.ToUser(),
                MaxSubscribers = maxSubscribers
            };

            await Tools.Database.Events.AddAsync(newEvent);

            embedBuilder = new EmbedBuilder().CreateSuccess($"Event {title} has been successfully added for {occurenceDateTime} with a max of {maxSubscribers} subscribers!");
            await ReplyAsync(string.Empty, false, embedBuilder.Build());
        }

        [Command("DeleteEvent")]
        public async Task DeleteEvent(string title)
        {
            EmbedBuilder embedBuilder;

            Event _event;
            if ((_event = Tools.Database.Events.FirstOrDefault(x => x.Guild.Id == Context.Guild.Id && x.Title == title)) != null)
            {
                if (_event.Author.Id != Context.User.Id)
                {
                    embedBuilder = new EmbedBuilder().CreateError("You are not allowed to modify someone else's event");
                    await ReplyAsync(string.Empty, false, embedBuilder.Build());
                    return;
                }

                Tools.Database.Events.Remove(_event);

                embedBuilder = new EmbedBuilder().CreateSuccess($"Successfully removed event {_event.Title}!");
                await ReplyAsync(string.Empty, false, embedBuilder.Build());
                return;
            }
            
            embedBuilder = new EmbedBuilder().CreateError($"Unable to find event {title}!");
            await ReplyAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}
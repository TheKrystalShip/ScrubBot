using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ScrubBot.Database.Domain;
using ScrubBot.Extensions;
using ScrubBot.Managers;
using ScrubBot.Tools;

using TheKrystalShip.DependencyInjection;

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
                return new ErrorResult($"An event with the name **{eventTitle}** already exists!");

            Event newEvent = new Event
            {
                Title = eventTitle,
                Description = description,
                Guild = Guild,
                OccurenceDate = occurenceDateTimeUTC.ToUniversalTime(),
                SubscribeMessageId = Context.Message.Id,
                Author = User,
                MaxSubscribers = maxSubscribers
            };

            await Database.Events.AddAsync(newEvent);

            return new SuccessResult($"Event **{eventTitle}** has been successfully created for **{occurenceDateTimeUTC}** with a max of **{maxSubscribers}** subscribers!");
        }

        [Command("CreateListEvent"), Summary("Create a new event")]
        public async Task<RuntimeResult> CreateListEvent(string eventTitle, string occurenceDate, string occurenceTime, int maxSubscribers, [Remainder] string description = "")
        {
            if (maxSubscribers < 1)
                return new ErrorResult("Max subscribers cannot be less than 1!");
            
            if (Database.Events.Any(x => x.Title == eventTitle && x.Guild.Id == Guild.Id))
                return new ErrorResult($"An event with the name **{eventTitle}** already exists!");
            
            if (!DateTime.TryParse($"{occurenceDate} {occurenceTime}", out var occurenceDateTime))
                return new ErrorResult($"Could not parse date ({occurenceDate}) and/or time ({occurenceTime})");
            
            Embed embed = EmbedFactory.Create(x =>
            {
                x.Title = eventTitle;
                x.Description = description;
                x.WithColor(Color.Orange);
                x.AddField("Occurence date", occurenceDateTime);
                x.AddField("Participants", $"1. {Context.User.Mention} (Author)");
            });

            //await Context.Channel.DeleteMessageAsync(Context.Message.Id); // Requires admin permissions
            var message = await ReplyAsync(embed);

            Event newEvent = new Event
            {
                Title = eventTitle,
                Description = description,
                Guild = Guild,
                OccurenceDate = occurenceDateTime.ToUniversalTime(),
                SubscribeMessageId = message.Id,
                Author = User,
                MaxSubscribers = maxSubscribers
            };

            await Database.Events.AddAsync(newEvent);
            ReactionManager reactionManager = (ReactionManager)Container.Get<IReactionManager>();
            await message.AddReactionsAsync(new IEmote[] { reactionManager.JoinEmoji, reactionManager.LeaveEmoji }, RequestOptions.Default);

            return new EmptyResult();
        }

        [Command("JoinEvent"), Summary("Join a specific event")]
        public async Task<RuntimeResult> JoinEvent(string eventTitle)
        {
            Event @event = Database.Events.FirstOrDefault(x => x.Title == eventTitle && x.Guild.Id == Guild.Id);

            if (@event is null)
                return new ErrorResult(CommandError.ObjectNotFound, $"No event with title **{eventTitle}** was found");
            
            if (@event.Author.Id == User.Id)
                return new ErrorResult("You cannot subscribe to your own event!");
            
            if (@event.Subscribers.Any(x => x.Id == User.Id))
                return new ErrorResult($"**{User.Username}** is already subscribed to **{eventTitle}**!");
            
            if (@event.Subscribers.Count == @event.MaxSubscribers)
                return new ErrorResult($"Event **{eventTitle}** is already full!");
            
            @event.Subscribers.Add(User);
            Database.Events.Update(@event);

            return new SuccessResult($"**{User.Username}** has successfully joined event **{eventTitle}** ({@event.Subscribers.Count}/{@event.MaxSubscribers})");
        }

        [Command("DeleteEvent"), Summary("Delete one of your events")]
        public async Task<RuntimeResult> DeleteEvent([Remainder]string eventTitle)
        {
            Event @event = Database.Events.FirstOrDefault(x => x.Title == eventTitle);

            if (@event is null || @event.Guild.Id != Guild.Id)
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

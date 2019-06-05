using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Domain;
using ScrubBot.Extensions;
using ScrubBot.Tools;
using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Managers
{
    public class ReactionManager : IReactionManager
    {
        protected enum EmojiAction { Join, Leave, None }
        protected IDbContext Database { get; }

        public readonly Emoji JoinEmoji; // ✅ //
        public readonly Emoji LeaveEmoji; // ❌ //

        public readonly List<Emoji> Emojis;

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();

            JoinEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Join").Value);
            LeaveEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Leave").Value);

            Emojis = new List<Emoji>
            {
                JoinEmoji,
                LeaveEmoji
            };
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            foreach (Emoji emoji in Emojis)
            {
                if (emoji.Name.Equals(reaction.Emote.Name)) {
                    return;
                }
            }

            IUserMessage message = await cacheable.GetOrDownloadAsync();

            if (message is null) {
                return;
            }

            if (!EventExists(message.Id, out Event @event))
            {
                if (!(reaction.User.GetValueOrDefault() is SocketGuildUser responder)) {
                    return;
                }

                Embed embed = EmbedFactory.Create(builder =>
                {
                    builder.CreateError("Could not subscribe you to the event. Please try again in a bit. " +
                        "If this error keeps appearing, ask the event owner to fix the event!");
                });

                await responder.SendMessageAsync(string.Empty, false, embed);
                return;
            }

            Emoji reactionEmoji = (Emoji)reaction.Emote;

            switch (DetermineEmojiAction(reactionEmoji.Name))
            {
                case EmojiAction.Join:
                {
                    if (@event.Subscribers.Count < @event.MaxSubscribers)
                    {
                        @event.Subscribers.Add(Database.Users.Find(reaction.UserId));
                        break;
                    }

                    if (!(reaction.User.Value is SocketGuildUser responder))
                        return;

                    Embed embed = EmbedFactory.Create(x =>
                    {
                        x.Title = "Error";
                        x.Description = "Could not subscribe you to the event. The event has hit the specified max subscriber limit!";
                        x.WithColor(Color.Red);
                    });

                    await responder.SendMessageAsync(null, false, embed);
                    return;
                }

                case EmojiAction.Leave:
                {
                    @event.Subscribers.Remove(Database.Users.Find(reaction.UserId));
                    break;
                }
            }

            Embed updatedEventEmbed = EmbedFactory.Create(builder =>
            {
                builder
                    .WithTitle(@event.Title)
                    .WithDescription(@event.Description)
                    .WithColor(Color.Orange)
                    .AddField("Occurence date", @event.OccurenceDate.ToString("f"));

                string participants = $"1. {@event.Author.Mention()} (Author)";

                for (int index = 0; index < @event.Subscribers.Count; index++) {
                    participants += $"\n{index + 2} {@event.Subscribers[index].Mention()}"; // TODO: Mention all subscribers
                }

                builder.AddField("Participants", participants);
            });

            await message.ModifyAsync(properties => properties.Embed = updatedEventEmbed);
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            //if (reaction.UserId == Client.CurrentUser.Id)
            return;
        }

        protected EmojiAction DetermineEmojiAction(string emojiName)
        {
            if (emojiName == JoinEmoji.Name) {
                return EmojiAction.Join;
            }

            if (emojiName == LeaveEmoji.Name) {
                return EmojiAction.Leave;
            }

            return EmojiAction.None;
        }

        protected bool EventExists(ulong eventMessageId, out Event @event)
        {
            @event = Database.Events.FirstOrDefault(x => x.SubscribeMessageId == eventMessageId);
            return @event != null;
        }
    }
}

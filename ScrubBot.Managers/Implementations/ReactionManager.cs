using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
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

            Emojis = new List<Emoji>
            {
                (JoinEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Join").Value)),
                (LeaveEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Leave").Value))
            };
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
            {
                return;
            }

            int matches = 0;
            foreach (Emoji emoji in Emojis)
            {
                if (string.CompareOrdinal(emoji.Name, reaction.Emote.Name) is 0)
                {
                    matches++;
                }
            }

            if (matches <= 0)
            {
                return;
            }

            IUserMessage message = await cacheable.GetOrDownloadAsync();

            if (message is null) 
            {
                return;
            }
            
            IUser user = reaction.User.GetValueOrDefault();

            if (!EventExists(message.Id, out Event @event))
            {
                if (!(user is SocketGuildUser responder)) 
                {
                    return;
                }

                Embed embed = EmbedFactory.Create(builder =>
                {
                    builder.CreateError("Could not subscribe you to the event. Please try again in a bit. " +
                        "If this error keeps appearing, ask the bot creator to confirm the event got created properly!");
                });

                await responder.SendMessageAsync(string.Empty, false, embed);
                return;
            }

            Emoji reactionEmoji = new Emoji(reaction.Emote.Name);

            switch (DetermineEmojiAction(reactionEmoji.Name))
            {
                case EmojiAction.Join:
                {
                    if (@event.Subscribers.Count < @event.MaxSubscribers)
                    {
                        if (@event.Author.Id == user.Id)
                        {
                            await user.SendMessageAsync(null, false, EmbedFactory.Create(x => x.CreateInfo("We'd like it if you didn't try subscribing to your own event. It doesn't work like that...")));
                            return;
                        }
                        
                        @event.Subscribers.Add(Database.Users.Find(user.Id));
                        break;
                    }

                    if (!(reaction.User.Value is SocketGuildUser responder))
                        return;

                    Embed embed = EmbedFactory.Create(x => x.CreateError("Could not subscribe you to the event. The event has hit the specified max subscriber limit!"));
                    await responder.SendMessageAsync(null, false, embed);
                    return;
                }

                case EmojiAction.Leave:
                {
                    @event.Subscribers.Remove(Database.Users.Find(reaction.UserId));
                    break;
                }
            }

            Embed updatedEventEmbed = EmbedFactory.Create(builder => builder.CreateEventEmbed(@event, socketMessageChannel));
            await message.ModifyAsync(properties => properties.Embed = updatedEventEmbed);
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
            {
                return;
            }
            
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
            @event = Database.Events.Where(x => x.SubscribeMessageId == eventMessageId).Include(x => x.Author).FirstOrDefault();
            return @event != null;
        }
    }
}

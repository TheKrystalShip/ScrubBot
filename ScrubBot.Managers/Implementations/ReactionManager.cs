using System;
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

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();

            JoinEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Join").Value);
            LeaveEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Leave").Value);
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (reaction.Emote.Name.Equals(JoinEmoji.Name) || reaction.Emote.Name.Equals(LeaveEmoji.Name)) // At the moment, only events have emoji mechanics
                return;

            var message = await cacheable.GetOrDownloadAsync();

            if (message is null)
                return;

            if (!EventExists(message.Id, out Event @event))
            {
                if (!(reaction.User.Value is SocketGuildUser responder))
                    return;

                Embed embed = EmbedFactory.Create(x =>
                {
                    x.Title = "Error";
                    x.Description = "Could not subscribe you to the event. Please try again in a bit. If this error keeps appearing, ask the event owner to fix the event!";
                    x.WithColor(Color.Red);
                });

                await responder.SendMessageAsync(null, false, embed);
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

            Embed updatedEventEmbed = EmbedFactory.Create(x =>
            {
                x.Title = @event.Title;
                x.Description = @event.Description;
                x.WithColor(Color.Orange);
                x.AddField("Occurence date", @event.OccurenceDate.ToString("f"));

                string participants = $"1. {@event.Author.Id.Mention()} (Author)";

                for (int index = 0; index < @event.Subscribers.Count; index++)
                    participants += $"\n{index + 2} {@event.Subscribers[index].Id.Mention()}";

                x.AddField("Participants", participants);
            });

            await Database.SaveChangesAsync();
            await message.ModifyAsync(properties => properties.Embed = updatedEventEmbed);
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            //if (reaction.UserId == Client.CurrentUser.Id)
            return;
        }

        protected EmojiAction DetermineEmojiAction(string emojiName) => emojiName == JoinEmoji.Name ? EmojiAction.Join : (emojiName == LeaveEmoji.Name ? EmojiAction.Leave : EmojiAction.None);

        protected bool EventExists(ulong eventMessageId, out Event @event)
        {
            @event = Database.Events.FirstOrDefault(x => x.SubscribeMessageId == eventMessageId);
            return @event != null;
        }
    }
}

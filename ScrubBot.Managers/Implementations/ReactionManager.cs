using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Database.Domain;
using ScrubBot.Tools;
using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Tools.Configuration;

namespace ScrubBot.Managers
{
    public class ReactionManager : IReactionManager
    {
        protected enum EmojiAction { Join, Leave, Delete, None }
        //protected DiscordSocketClient Client;
        protected IDbContext Database { get; }

        protected readonly Emoji JoinEmoji; // ✅ //
        protected readonly Emoji LeaveEmoji; // ❌ //
        protected readonly Emoji DeleteEmoji; // 💥 //

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();
            //Client = Container.Get<DiscordSocketClient>();

            var settingsEmoji = Configuration.GetSection("Bot").GetSection("EventEmoji").GetChildren().ToArray();

            JoinEmoji = new Emoji(settingsEmoji[0].Value);
            LeaveEmoji = new Emoji(settingsEmoji[1].Value);
            DeleteEmoji = new Emoji(settingsEmoji[2].Value);
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            await cacheable.DownloadAsync();

            if (!cacheable.HasValue)
                return;

            if (!EventExists(cacheable.Value.Id, out Event @event))
            {
                SocketGuildUser responder = (SocketGuildUser)reaction.User.Value;
                if (responder is null)
                {
                    await cacheable.Value.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                    return;
                }

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
                    @event.Subscribers.Add(Database.Users.Find(reaction.UserId));
                    break;

                case EmojiAction.Leave:
                    @event.Subscribers.Remove(Database.Users.Find(reaction.UserId));
                    break;

                case EmojiAction.Delete:
                    if (reaction.UserId != @event.Author.Id)
                    {
                        await cacheable.Value.RemoveReactionAsync(reactionEmoji, reaction.User.Value);
                        return;
                    }
                    await cacheable.Value.DeleteAsync();
                    return;
            }

            //SocketGuild guild = ; // TODO: Get the guild of this event
            Embed newEventEmbed = EmbedFactory.Create(x =>
            {
                x.Title = @event.Title;
                x.Description = @event.Description;
                x.WithColor(Color.DarkOrange);

                string participants = "1. ";
                //for (int index = 0; index < @event.Subscribers.Count; index++)
                //    participants += $"{index + 2} {@event.Subscribers[index].}"; // TODO: Mention all subscribers
                x.AddField("Participants", participants);
            });

            await Database.SaveChangesAsync();
            await cacheable.Value.RemoveReactionAsync(reactionEmoji, reaction.User.Value);
            await cacheable.Value.ModifyAsync(properties => properties.Embed = newEventEmbed);
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            //if (reaction.UserId == Client.CurrentUser.Id)
                return;
        }

        protected EmojiAction DetermineEmojiAction(string emojiName)
        {
            if (emojiName == JoinEmoji.Name)
                return EmojiAction.Join;

            if (emojiName == LeaveEmoji.Name)
                return EmojiAction.Leave;

            return emojiName == DeleteEmoji.Name ? EmojiAction.Delete : EmojiAction.None;
        }

        protected bool EventExists(ulong eventMessageId, out Event @event)
        {
            @event = Database.Events.FirstOrDefault(x => x.SubscribeMessageId == eventMessageId);
            return @event != null;
        }
    }
}

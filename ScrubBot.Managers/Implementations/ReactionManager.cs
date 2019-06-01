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
        protected IDbContext Database { get; }

        public readonly Emoji JoinEmoji; // ✅ //
        public readonly Emoji LeaveEmoji; // ❌ //
        public readonly Emoji DeleteEmoji; // 💥 //

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();

            JoinEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Join").Value);
            LeaveEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Leave").Value);
            DeleteEmoji = new Emoji(Configuration.GetSection("Bot:EventEmoji:Delete").Value);
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (reaction.Emote.Name.Equals(JoinEmoji.Name) || reaction.Emote.Name.Equals(LeaveEmoji.Name) || reaction.Emote.Name.Equals(DeleteEmoji.Name)) // At the moment, only events have emoji mechanics
                return;

            var message = await cacheable.GetOrDownloadAsync();

            if (message is null)
                return;

            if (!EventExists(message.Id, out Event @event))
            {
                if (!(reaction.User.Value is SocketGuildUser responder))
                {
                    //await message.RemoveReactionAsync(reaction.Emote, reaction.User.Value); // Requires Admin permission
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
                {
                    if (@event.Subscribers.Count < @event.MaxSubscribers)
                    {
                        @event.Subscribers.Add(Database.Users.Find(reaction.UserId));
                        break;
                    }

                    if (!(reaction.User.Value is SocketGuildUser responder))
                    {
                        //await message.RemoveReactionAsync(reaction.Emote, reaction.User.Value); // Requires Admin permission
                        return;
                    }

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

                case EmojiAction.Delete:
                {
                    if (reaction.UserId != @event.Author.Id)
                    {
                        //await message.RemoveReactionAsync(reactionEmoji, reaction.User.Value); // Requires Admin permission
                        return;
                    }
                    await message.DeleteAsync();
                    return;
                }
            }

            Embed updatedEventEmbed = EmbedFactory.Create(x => // TODO: Improve user loading to reduce execution times
            {
                x.Title = @event.Title;
                x.Description = @event.Description;
                x.WithColor(Color.Orange);
                x.AddField("Occurence date", @event.OccurenceDate.ToString("f"));

                string participants = $"1. {socketMessageChannel.GetUserAsync(@event.Author.Id).Result.Mention} (Author)";

                for (int index = 0; index < @event.Subscribers.Count; index++)
                    participants += $"\n{index + 2} {socketMessageChannel.GetUserAsync(@event.Subscribers[index].Id).Result.Mention}"; // TODO: Mention all subscribers

                x.AddField("Participants", participants);
            });

            await Database.SaveChangesAsync();
            //await message.RemoveReactionAsync(reactionEmoji, reaction.User.Value); // Requires admin permissions
            await message.ModifyAsync(properties => properties.Embed = updatedEventEmbed);
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

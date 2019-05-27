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
        protected IDbContext Database { get; }
        protected IPrefixManager PrefixManager { get; private set; }
        protected Guild Guild { get; private set; }
        protected User User { get; private set; }
        protected readonly Emoji JoinEmoji; // ✅ //
        protected readonly Emoji LeaveEmoji; // ❌ //
        protected readonly Emoji DeleteEmoji; // 💥 //

        public ReactionManager()
        {
            Database = Container.Get<IDbContext>();
            Emoji[] settingsEmoji = (Emoji[])Configuration.GetSection("ReactionEmoji").GetChildren().ToArray();

            JoinEmoji = settingsEmoji[0];
            LeaveEmoji = settingsEmoji[1];
            DeleteEmoji = settingsEmoji[2];
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            //Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been added to a message in {socketMessageChannel.Name}"));

            await cacheable.GetOrDownloadAsync();

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

            if (Equals(reactionEmoji.Name, JoinEmoji.Name))
            {
                @event.Subscribers.Add(Database.Users.Find(reaction.UserId));
                await Database.SaveChangesAsync();
            }
            else if (Equals(reactionEmoji.Name, LeaveEmoji.Name))
            {
                @event.Subscribers.Remove(Database.Users.Find(reaction.UserId));
                await Database.SaveChangesAsync();
            }
            else if (Equals(reactionEmoji.Name, DeleteEmoji.Name))
            {
                if (reaction.UserId != @event.Author.Id)
                    return;

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

            await cacheable.Value.ModifyAsync(properties => properties.Embed = newEventEmbed);

            await Task.CompletedTask;
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel, SocketReaction reaction)
        {
            Console.WriteLine(new LogMessage(LogSeverity.Info, GetType().Name, $"Reaction {reaction.Emote.Name} has been removed to a message in {socketMessageChannel.Name}"));

            await Task.CompletedTask;
        }



        private bool EventExists(ulong eventMessageId, out Event @event)
        {
            @event = Database.Events.FirstOrDefault(x => x.SubscribeMessageId == eventMessageId);
            return @event != null;
        }
    }
}

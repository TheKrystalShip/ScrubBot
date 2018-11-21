using System.Linq;
using Discord.WebSocket;
using ScrubBot.Core;
using ScrubBot.Database;
using ScrubBot.Domain;

using System.Threading.Tasks;
using Discord;
using ScrubBot.Extensions;

namespace ScrubBot.Handlers
{
    public class ServiceHandler
    {
        private readonly Bot _client;
        private readonly SQLiteContext _dbContext;

        public ServiceHandler(Bot client, SQLiteContext dbContext)
        {
            _client = client;
            _dbContext = dbContext;
        }

        public async Task OnBirthdayServiceTriggerAsync(User user)
        {
            await _client.GetUser(user.Id).SendMessageAsync(string.Empty, false, new EmbedBuilder().CreateMessage("Hey kanjer, dit is je verjaardag!",
                                                                                                                  $"Van harte gefeliciflapstaart met je verjaardag **{user.Username}**! Hiep hiep hoera en een fijne dag!"));
            await Task.CompletedTask;
        }

        public async Task OnEventServiceTriggerAsync(Event @event)
        {
            foreach (var subscriber in @event.Subscribers)
                await _client.GetUser(subscriber.Id).SendMessageAsync(string.Empty, false, new EmbedBuilder().CreateMessage("Event reminder", $"Dear {subscriber.Username},\t" +
                                                                                                                                              $"**{@event.Author.Username}{(@event.Author.Username.Last() == 's' ? "'" : "'s")}** event **{@event.Title}** is about to start!"));

            await _client.GetUser(@event.Author.Id).SendMessageAsync(string.Empty, false, new EmbedBuilder().CreateMessage("Event reminder", $"Your event **{@event.Title}** is about to start!"));

            await Task.CompletedTask;
        }
    }
}

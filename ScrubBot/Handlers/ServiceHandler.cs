using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;

using ScrubBot.Core;
using ScrubBot.Database.Domain;
using ScrubBot.Database.SQLite;
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
            await _client
                .GetUser(user.Id)
                .SendMessageAsync(new EmbedBuilder()
                    .CreateMessage("Hey kanjer, dit is je verjaardag!", $"Van harte gefeliciflapstaart met je verjaardag **{user.Username}**! Hiep hiep hoera en een fijne dag!"));
        }

        public async Task OnEventServiceTriggerAsync(Event @event)
        {
            List<Task<IUserMessage>> tasks = new List<Task<IUserMessage>>();

            foreach (var subscriber in @event.Subscribers)
            {
                tasks.Add(_client
                    .GetUser(subscriber.Id)
                    .SendMessageAsync(new EmbedBuilder()
                        .CreateMessage("Event reminder", $"Dear {subscriber.Username},\t" + $"**{@event.Author.Username}{(@event.Author.Username.Last() == 's' ? "'" : "'s")}** event **{@event.Title}** is about to start!"))
                );
            }

            await _client
                .GetUser(@event.Author.Id)
                .SendMessageAsync(new EmbedBuilder()
                    .CreateMessage("Event reminder", $"Your event **{@event.Title}** is about to start!"));

            await Task.WhenAll(tasks);
        }
    }
}

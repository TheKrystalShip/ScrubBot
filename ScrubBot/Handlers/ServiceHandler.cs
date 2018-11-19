using Discord.WebSocket;
using ScrubBot.Core;
using ScrubBot.Database;
using ScrubBot.Domain;

using System.Threading.Tasks;

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
            // Logic

            await Task.CompletedTask;
        }

        public async Task OnEventServiceTriggerAsync(Event @event)
        {
            // Logic

            await Task.CompletedTask;
        }
    }
}

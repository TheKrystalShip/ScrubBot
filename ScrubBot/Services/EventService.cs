
using ScrubBot.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Discord;
using Discord.WebSocket;
using ScrubBot.Domain;
using Microsoft.EntityFrameworkCore;
using ScrubBot.Tools;

namespace ScrubBot.Services
{
    public class EventService : Service
    {
        protected override Timer Timer { get; set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;
        private readonly SQLiteContext _dbContext;
        private readonly DiscordSocketClient _client;

        public EventService(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
            DiscordSocketClient _client = Container.Get<DiscordSocketClient>();
        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        public override void Loop(object state)
        {
            List<Event> events = _dbContext.Events
                .Where(x => x.OccurenceDate <= DateTimeOffset.UtcNow)
                .Include(x => x.Author)
                .Include(x => x.Subscribers)
                .Take(10)
                .ToList();

            if (events.Count is 0) return;

            foreach (var _event in events)
            {
                foreach (var user in _event.Subscribers)
                    _client.GetUser(user.Id)?.SendMessageAsync("You can fuck right off").Wait();
                _client.GetUser(_event.Author.Id)?.SendMessageAsync("You can fuck right off").Wait();
            }

            Tick?.Invoke(this);
        }

        public override void Dispose()
        {
            Timer.Dispose();
            Stop.Invoke(this);
        }
    }
}

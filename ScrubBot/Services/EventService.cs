using ScrubBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ScrubBot.Domain;
using Microsoft.EntityFrameworkCore;
using ScrubBot.Extensions;
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
            _client = Container.Get<DiscordSocketClient>();
        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        public override void Loop(object state) // TODO: Replace Try-Catch with proper exception catching
        {
            try
            {
                List<Event> events = _dbContext.Events
                    .Where(x => x.OccurenceDate <= DateTime.UtcNow)
                    .Include(x => x.Author)
                    .Include(x => x.Subscribers)
                    .Take(10)
                    .ToList();

                if (events.Count is 0) return;

                List<Task> tasks = new List<Task>();

                foreach (var _event in events)
                {
                    foreach (var user in _event.Subscribers)
                        tasks.Add(Task.Run(async () => await _client.GetUser(user.Id).
                            SendMessageAsync(string.Empty,
                                             false,
                                             new EmbedBuilder().CreateMessage($"(Event) {_event.Title}",
                                                                     $"Dear {user.Username},\t" +
                                                                     $"This is a reminder that event {_event.Title} is about to start!").Build())));
                    tasks.Add(Task.Run(async () => await _client.GetUser(_event.Author.Id).
                        SendMessageAsync(string.Empty,
                                         false,
                                         new EmbedBuilder().CreateMessage($"(Event) {_event.Title}",
                                                                          $"This is a reminder that your event {_event.Title} is about to start!").Build())));
                }

                Task.WhenAll(tasks).Wait();
                _dbContext.Events.RemoveRange(events);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Tick?.Invoke(this);
        }

        public override void Dispose()
        {
            Timer.Dispose();
            Stop?.Invoke(this);
        }
    }
}

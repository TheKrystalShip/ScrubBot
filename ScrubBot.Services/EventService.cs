using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ScrubBot.Database.Domain;
using ScrubBot.Database.SQLite;

namespace ScrubBot.Services
{
    public class EventService : IService
    {
        private readonly SQLiteContext _dbContext;

        public Timer Timer { get; set; }
        public event Action Start;
        public event Action Stop;
        public event Action Tick;
        public event Func<Event, Task> Trigger;

        public EventService(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke();
        }

        public void Loop(object state)
        {
            List<Event> events = _dbContext.Events
                .Where(x => x.IsDue())
                .Include(x => x.Author)
                .Include(x => x.Subscribers)
                .Take(10)
                .ToList();

            if (events.Count is 0)
            {
                return;
            }

            foreach (Event @event in events)
            {
                Trigger?.Invoke(@event);
            }

            Tick?.Invoke();
        }

        public void Dispose()
        {
            Timer.Dispose();
            Stop?.Invoke();
        }
    }
}

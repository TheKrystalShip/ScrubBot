
using ScrubBot.Database;

using System;
using System.Threading;

namespace ScrubBot.Services
{
    public class EventService : Service
    {
        protected override Timer Timer { get; set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;
        private readonly SQLiteContext _dbContext;

        public EventService(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        public override void Loop(object state)
        {

            Tick?.Invoke(this);
        }

        public override void Dispose()
        {
            Timer.Dispose();
            Stop.Invoke(this);
        }
    }
}

using System;
using System.Threading;

namespace ScrubBot.Services
{
    public abstract class Service : IDisposable
    {
        protected abstract Timer Timer { get; set; }
        public abstract event Action<Service> Start;
        public abstract event Action<Service> Stop;
        public abstract event Action<Service> Tick;

        public abstract void Init(int startDelay = 0, int interval = 1000);
        public abstract void Loop(object state);
        public abstract void Dispose();
    }
}

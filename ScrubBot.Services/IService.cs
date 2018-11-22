using System;
using System.Threading;

namespace ScrubBot.Services
{
    public interface IService : IDisposable
    {
        Timer Timer { get; set; }

        event Action Start;
        event Action Tick;
        event Action Stop;

        void Loop(object state);
        void Init(int startDelay = 0, int interval = 1000);
    }
}

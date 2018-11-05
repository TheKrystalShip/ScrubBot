using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Discord.WebSocket;
using ScrubBot.Database;
using ScrubBot.Tools;

namespace ScrubBot.Services
{
    class BirthdayService : Service
    {
        protected override Timer Timer { get; set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;
        private readonly SQLiteContext _dbContext;
        private readonly DiscordSocketClient _client;

        public BirthdayService(SQLiteContext dbContext)
        {
            _dbContext = dbContext;
            _client = Container.Get<DiscordSocketClient>();
        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        public override void Loop(object state)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void Dispose()
        {

        }
    }
}

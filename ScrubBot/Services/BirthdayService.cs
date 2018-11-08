using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using ScrubBot.Database;
using ScrubBot.Domain;
using ScrubBot.Extensions;
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
            Loop(null);
            Start?.Invoke(this);
        }

        public override void Loop(object state)
        {
            try
            {
                var today = DateTime.UtcNow;
                List<User> birthdayBois = _dbContext.Users
                    .Where(x => x.Birthday.Month == today.Month & x.Birthday.Day == today.Day)
                    .Take(10)
                    .ToList();

                if (birthdayBois.Count is 0) return;

                List<Task> tasks = new List<Task>();

                foreach (var birthdayBoi in birthdayBois)
                {
                    if (birthdayBoi.Birthday == DateTime.MinValue.Date) continue;

                    tasks.Add(Task.Run(async () => await _client.GetUser(birthdayBoi.Id).SendMessageAsync(string.Empty,
                            false,
                            new EmbedBuilder().CreateMessage("Hey kanjer, dit is je verjaardag!",
                                                             $"Van harte gefeliciflapstaart met je verjaardag {birthdayBoi.Username}! Hiep hiep hoera en een fijne dag!"))));
                }

                Task.WhenAll(tasks).Wait();
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

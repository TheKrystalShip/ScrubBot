using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ScrubBot.Database.Domain;
using ScrubBot.Database.SQLite;

namespace ScrubBot.Services
{
    public class BirthdayService : IService
    {
        private readonly SQLiteContext _dbContext;

        public Timer Timer { get; set; }
        public event Action Start;
        public event Action Stop;
        public event Action Tick;
        public event Func<User, Task> Trigger;

        public BirthdayService(SQLiteContext dbContext)
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
            List<User> birthdayBois = _dbContext.Users
                .Where(x => x.IsBirthdayToday())
                .Take(10)
                .ToList();

            if (birthdayBois.Count is 0)
            {
                return;
            }

            foreach (User birthdayBoi in birthdayBois)
            {
                Trigger?.Invoke(birthdayBoi);
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

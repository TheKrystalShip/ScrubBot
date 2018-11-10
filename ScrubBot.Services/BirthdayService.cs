using ScrubBot.Domain;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ScrubBot.Services
{
    public class BirthdayService : IService
    {
        public Timer Timer { get; set; }
        public event Action Start;
        public event Action Stop;
        public event Action Tick;
        public event Func<User, Task> Trigger;

        public BirthdayService()
        {

        }

        public void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke();
        }

        public void Loop(object state)
        {
            try
            {
                //var today = DateTime.UtcNow;
                //List<User> birthdayBois = _dbContext.Users
                //    .Where(x => x.Birthday.Month == today.Month & x.Birthday.Day == today.Day)
                //    .Take(10)
                //    .ToList();

                //if (birthdayBois.Count is 0) return;

                //List<Task> tasks = new List<Task>();

                //foreach (var birthdayBoi in birthdayBois)
                //{
                //    if (birthdayBoi.Birthday == DateTime.MinValue.Date) continue;

                //    tasks.Add(Task.Run(async () => await _client.GetUser(birthdayBoi.Id).SendMessageAsync(string.Empty,
                //            false,
                //            new EmbedBuilder().CreateMessage("Hey kanjer, dit is je verjaardag!",
                //                                             $"Van harte gefeliciflapstaart met je verjaardag {birthdayBoi.Username}! Hiep hiep hoera en een fijne dag!"))));
                //}

                //Task.WhenAll(tasks).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Tick?.Invoke();
            }
        }

        public void Dispose()
        {
            Timer.Dispose();
            Stop?.Invoke();
        }
    }
}

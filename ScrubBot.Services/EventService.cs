using ScrubBot.Domain;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScrubBot.Services
{
    public class EventService : IService
    {
        public Timer Timer { get; set; }
        public event Action Start;
        public event Action Stop;
        public event Action Tick;
        public event Func<Event, Task> Trigger;

        public EventService()
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
                //List<Event> events = _dbContext.Events
                //    .Where(x => x.OccurenceDate <= DateTime.UtcNow)
                //    .Include(x => x.Author)
                //    .Include(x => x.Subscribers)
                //    .Take(10)
                //    .ToList();

                //if (events.Count is 0) return;

                //List<Task> tasks = new List<Task>();

                //foreach (var _event in events)
                //{
                //    foreach (var user in _event.Subscribers)
                //        tasks.Add(Task.Run(async () => await _client.GetUser(user.Id).
                //            SendMessageAsync(string.Empty,
                //                             false,
                //                             new EmbedBuilder().CreateMessage("Event reminder",
                //                                                     $"Dear {user.Username},\t" +
                //                                                     $"**{_event.Author.Username}{(_event.Author.Username.Last() == 's' ? "'" : "'s")}** event **{_event.Title}** is about to start!").Build())));
                //    tasks.Add(Task.Run(async () => await _client.GetUser(_event.Author.Id).
                //       SendMessageAsync(string.Empty,
                //                        false,
                //                        new EmbedBuilder().CreateMessage("Event reminder",
                //                                                         $"Your event **{_event.Title}** is about to start!").Build())));
                //}

                //Task.WhenAll(tasks).Wait();
                //_dbContext.Events.RemoveRange(events);
                //_dbContext.SaveChanges();
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

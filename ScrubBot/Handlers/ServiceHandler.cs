using Discord.WebSocket;

using ScrubBot.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrubBot.Handlers
{
    class ServiceHandler
    {
        private static List<IService> ServiceList { get; set; } = new List<IService>();
        private const int InitializeDelay = 5000;
        private const int Interval = 1000;

        public ServiceHandler(DiscordSocketClient client) => Initialize(client);

        //private void OnStartService(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Started");

        //private void OnServiceTick(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Ticked");

        //private void OnServiceStop(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Stopped");

        public static void StartAllLoops()
        {
            //LogHandler.WriteLine(LogTarget.Console, "Starting all loops...");

            foreach (IService service in ServiceList)
                service.Init(InitializeDelay, Interval);
        }

        public static void StopAllLoops()
        {
            //LogHandler.WriteLine(LogTarget.Console, "Stopping all loops...");

            foreach (IService service in ServiceList)
                service.Dispose();
        }

        private void HandleEvents<T>(T service) where T : IService
        {
            //service.Start += OnServiceStart;
            //service.Tick += OnServiceTick;
            //service.Stop += OnServiceStop;
        }

        private void RegisterService<T>(T service) where T : IService
        {
            HandleEvents(service);
            ServiceList.Add(service);
        }

        private async void Initialize(DiscordSocketClient client)
        {
            // RegisterService(new YourService());

            //StartAllLoops();

            await Task.CompletedTask;
        }
    }
}

using System;
using System.Collections.Generic;
using Discord.WebSocket;
using ScrubBot.Services;

namespace ScrubBot.Handlers
{
    class ServiceHandler
    {
        private static List<Service> ServiceList { get; set; } = new List<Service>();
        private const int InitializeDelay = 5000;
        private const int Interval = 1000;

        public ServiceHandler(DiscordSocketClient client) => Initialize(client);

        //private void OnStartService(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Started");

        //private void OnServiceTick(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Ticked");

        //private void OnServiceStop(Service service) => LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Stopped");

        public static void StartAllLoops()
        {
            //LogHandler.WriteLine(LogTarget.Console, "Starting all loops...");

            foreach (Service service in ServiceList)
                service.Initialize(InitializeDelay, Interval);
        }

        public static void StopAllLoops()
        {
            //LogHandler.WriteLine(LogTarget.Console, "Stopping all loops...");

            foreach (Service service in ServiceList)
                service.Dispose();
        }

        private void HandleEvents<T>(T service) where T : Service
        {
            //service.Start += OnServiceStart;
            //service.Tick += OnServiceTick;
            //service.Stop += OnServiceStop;
        }

        private void RegisterService<T>(T service) where T : Service
        {
            HandleEvents(service);
            ServiceList.Add(service);
        }

        private async void Initialize(DiscordSocketClient client)
        {
            // RegisterService(new YourService());

            //StartAllLoops();
        }
    }
}
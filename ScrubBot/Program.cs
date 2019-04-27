using System;
using System.Threading.Tasks;

using ScrubBot.Core;

namespace ScrubBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "ScrubBot";

            await BotBuilder
                .UseStartup<Startup>()
                .ConfigureContainer()
                .ConfigureSettings()
                .ConfigureDatabase()
                .ConfigureClient()
                .ConfigureEvents()
                .ConfigureServices()
                .InitAsync();
        }
    }
}

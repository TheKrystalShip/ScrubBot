using System;
using System.Threading.Tasks;

using ScrubBot.Core;

namespace ScrubBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "ScrubBot";
            await BotBuilder
                .UseStartup<Startup>()
                .ConfigureContainer()
                .ConfigureDatabase()
                .ConfigureClient()
                .ConfigureEvents()
                .ConfigureServices()
                .InitAsync();
        }
    }
}

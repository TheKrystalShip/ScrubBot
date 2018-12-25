using System.Threading.Tasks;

using ScrubBot.Core;

namespace ScrubBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await BotBuilder
                .UseStartup<Startup>()
                .ConfigureDatabase()
                .ConfigureClient()
                .ConfigureServices()
                .InitAsync();
        }
    }
}

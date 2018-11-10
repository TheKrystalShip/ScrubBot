using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await BotBuilder
                .UseStartup<Startup>()
                .ConfigureDatabase()
                .ConfigureServices()
                .ConfigureBot()
                .InitAsync();
        }
    }
}

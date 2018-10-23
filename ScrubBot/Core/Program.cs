using ScrubBot.Extensions;

using System.Threading.Tasks;

namespace ScrubBot
{
    public class Program
    {
        private static Bot _scrubBot;

        public static async Task Main(string[] args)
        {
            await (_scrubBot = new Bot())
                .HookEvents()
                .InitAsync()
                .DelayIndefinetly();
        }
    }
}

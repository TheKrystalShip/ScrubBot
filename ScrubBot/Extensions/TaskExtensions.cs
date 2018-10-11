using System.Threading.Tasks;

namespace ScrubBot.Extensions
{
    public static class TaskExtensions
    {
        public static async Task DelayIndefinetly(this Task task)
        {
            await Task.Delay(-1);
        }
    }
}

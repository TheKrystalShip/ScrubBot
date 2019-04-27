using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace ScrubBot.Middlewares
{
    public class Test2Middleware : IMiddleware
    {
        public async Task InitAsync(ICommandContext context)
        {
            Console.WriteLine("Test2Middleware initialized and working");
            await Task.CompletedTask;
        }
    }
}

using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace ScrubBot.Middlewares
{
    public class TestMiddleware : IMiddleware
    {
        public async Task InitAsync(ICommandContext context)
        {
            Console.WriteLine("TestMiddleware initialized and working");
            await Task.CompletedTask;
        }
    }
}

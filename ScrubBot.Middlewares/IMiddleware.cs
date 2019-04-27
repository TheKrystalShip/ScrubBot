using System.Threading.Tasks;
using Discord.Commands;

namespace ScrubBot.Middlewares
{
    public interface IMiddleware
    {
        Task InitAsync(ICommandContext context);
    }
}

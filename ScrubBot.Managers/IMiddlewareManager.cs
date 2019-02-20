using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using ScrubBot.Middlewares;

namespace ScrubBot.Managers
{
    public interface IMiddlewareManager
    {
        IEnumerable<IMiddleware> GetMiddlewares();
        Task InitAsync(ICommandContext context);
    }
}

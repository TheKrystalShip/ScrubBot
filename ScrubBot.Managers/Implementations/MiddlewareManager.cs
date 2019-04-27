using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord.Commands;

using ScrubBot.Middlewares;

namespace ScrubBot.Managers
{
    public class MiddlewareManager : IMiddlewareManager
    {
        private readonly IEnumerable<IMiddleware> _middlewares;

        public MiddlewareManager()
        {
            _middlewares = GetMiddlewares();
        }
        
        public IEnumerable<IMiddleware> GetMiddlewares()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => typeof(IMiddleware).IsAssignableFrom(type)) // Ensures that object can be cast to interface
                .Where(type => !type.IsAbstract && !type.IsGenericType && type.GetConstructor(new Type[0]) != null) // Ensures that type can be instantiated
                .Select(type => (IMiddleware)Activator.CreateInstance(type)) // Create instances
                .ToList();
        }
        
        public async Task InitAsync(ICommandContext context)
        {
            List<Task> tasks = new List<Task>();

            foreach (IMiddleware middleware in _middlewares)
            {
                tasks.Add(middleware.InitAsync(context));
            }

            await Task.WhenAll(tasks);
        }
    }
}

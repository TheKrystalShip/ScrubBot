using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Handlers;
using ScrubBot.Services;

namespace ScrubBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ConversionHandler>();
            services.AddSingleton<EventHandler>();
            services.AddSingleton<ServiceHandler>();
            services.AddSingleton<PrefixHandler>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<EventService>();

            return services;
        }
    }
}

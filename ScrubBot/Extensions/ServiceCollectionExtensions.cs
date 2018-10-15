using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Services;

namespace ScrubBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ServiceHandler>();
            services.AddSingleton<PrefixHandler>();

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<ChannelManager>();
            services.AddSingleton<EventManager>();
            services.AddSingleton<GuildManager>();
            services.AddSingleton<RoleManager>();
            services.AddSingleton<UserManager>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<EventService>();

            return services;
        }

        public static IServiceCollection AddTools(this IServiceCollection services)
        {
            services.AddSingleton<Tools>();

            return services;
        }
    }
}

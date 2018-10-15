using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Database;
using ScrubBot.Handlers;
using ScrubBot.Managers;

using System;

namespace ScrubBot.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static void Init(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<SQLiteContext>().MigrateDatabase();
            serviceProvider.GetRequiredService<ChannelManager>();
            serviceProvider.GetRequiredService<EventManager>();
            serviceProvider.GetRequiredService<GuildManager>();
            serviceProvider.GetRequiredService<RoleManager>();
            serviceProvider.GetRequiredService<UserManager>();

            serviceProvider.GetRequiredService<ServiceHandler>();
        }
    }
}

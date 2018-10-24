using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ScrubBot.Database;
using ScrubBot.Handlers;
using ScrubBot.Managers;
using ScrubBot.Services;

using System;

namespace ScrubBot
{
    public static class Container
    {
        private static ServiceCollection _services;
        private static IServiceProvider _serviceProvider => _services.BuildServiceProvider();

        static Container()
        {
            _services = new ServiceCollection();

            _services.AddDbContext<SQLiteContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SQLite"));
            });
            _services.AddSingleton<ServiceHandler>();
            _services.AddSingleton<PrefixHandler>();
            _services.AddSingleton<ChannelManager>();
            _services.AddSingleton<GuildManager>();
            _services.AddSingleton<RoleManager>();
            _services.AddSingleton<UserManager>();
            _services.AddSingleton<EventService>();
            _services.AddSingleton<Tools>();
        }

        public static void Init()
        {
            IServiceProvider serviceProvider = _services.BuildServiceProvider();

            serviceProvider.GetRequiredService<ChannelManager>();
            serviceProvider.GetRequiredService<GuildManager>();
            serviceProvider.GetRequiredService<RoleManager>();
            serviceProvider.GetRequiredService<UserManager>();
            serviceProvider.GetRequiredService<ServiceHandler>();
        }

        public static void Add<T>() where T : class
        {
            Add(typeof(T));
        }

        public static void Add<T>(T type) where T : class
        {
            _services.AddSingleton<T>(type);
        }

        public static void Add(Type type)
        {
            _services.AddSingleton(type);
        }

        public static T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        public static object Get(Type type)
        {
            object value = _serviceProvider.GetService(type);

            if (value is null)
            {
                Add(type);
                return _serviceProvider.GetRequiredService(type);
            }

            return value;
        }

        public static IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ScrubBot.Tools
{
    public static class Container
    {
        private static ServiceCollection _services;
        private static IServiceProvider _serviceProvider => _services.BuildServiceProvider();

        static Container()
        {
            _services = new ServiceCollection();
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

        public static void Add<T, I>() where T : class where I : class, T
        {
            _services.AddSingleton<T, I>();
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

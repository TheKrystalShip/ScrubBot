using System;

using ScrubBot.IoC;

namespace ScrubBot.Tools
{
    public static class Container
    {
        private static ServiceResolver _serviceResolver;

        static Container()
        {
            _serviceResolver = new ServiceResolver();
        }

        public static void Add<T>() where T : class
        {
            Add(typeof(T));
        }

        public static void Add<T>(T type) where T : class
        {
            _serviceResolver.Register<T>(type);
        }

        public static void Add<T, I>() where T : class where I : class, T
        {
            _serviceResolver.Register<T, I>();
        }

        public static T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        private static object Get(Type type)
        {
            return _serviceResolver.GetService(type);
        }

        public static IServiceProvider GetServiceProvider()
        {
            return _serviceResolver;
        }
    }
}

using System.Collections.Concurrent;

namespace ScrubBot.Tools
{
    /// <summary>
    /// In-memory store (not the nicest solution, but it gets the job done)
    /// </summary>
    public static class Store
    {
        private static readonly ConcurrentDictionary<object, object> _store;

        static Store()
        {
            _store = new ConcurrentDictionary<object, object>();
        }

        public static void Set(object key, object value)
        {
            _store.AddOrUpdate(key, value, (oldKey, oldValue) => value);
        }

        public static void Set<T>(object value)
        {
            _store.AddOrUpdate(typeof(T), value, (oldKey, oldValue) => value);
        }

        public static void Set<T>(T value)
        {
            _store.AddOrUpdate(typeof(T), value, (oldKey, oldValue) => value);

        }

        public static T Get<T>(object key)
        {
            if (_store.TryGetValue(key, out object value))
            {
                return (T)value;
            };

            return default;
        }

        public static object Get(object key)
        {
            if (_store.TryGetValue(key, out object value))
            {
                return value;
            };

            return default;
        }

        public static T Get<T>()
        {
            return (T) _store[typeof(T)];
        }
    }
}

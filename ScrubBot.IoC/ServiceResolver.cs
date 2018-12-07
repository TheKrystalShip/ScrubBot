using System;
using System.Collections.Generic;

namespace ScrubBot.IoC
{
    public class ServiceResolver : IServiceResolver
    {
        public IDependencyInjector DependencyInjector { get; set; }

        private Dictionary<Type, object> _store;
        private Dictionary<Type, Type> _bindings;

        public ServiceResolver()
        {
            this.DependencyInjector = new DependencyInjector(this);
            _store = new Dictionary<Type, object>();
            _bindings = new Dictionary<Type, Type>();
        }

        public ServiceResolver(IDependencyInjector injector)
        {
            this.DependencyInjector = injector;
            _store = new Dictionary<Type, object>();
            _bindings = new Dictionary<Type, Type>();
        }

        public T Resolve<T>()
        {
            return (T) GetService(typeof(T));
        }

        // IServiceProvider for compatibility with Microsoft.Extensions.DependencyInjection
        public object GetService(Type type)
        {
            // Return stored instance if already registered
            if (_store.ContainsKey(type))
            {
                return _store[type];
            }

            if (!_bindings.ContainsKey(type))
            {
                return DependencyInjector.GetInjectedInstance(type);
            }
            else
            {
                Type dest = _bindings[type];

                // Create new instance of this type
                object obj = DependencyInjector.GetInjectedInstance(dest);

                // Store for future use
                _store.Add(dest, obj);

                return obj;
            }            
        }
        
        // Register type with implementation
        public void Register<TFrom, TTo>()
        {
            _bindings.Add(typeof(TFrom), typeof(TTo));
        }

        // Register type with same implementation
        public void Register<TType>()
        {
            Register<TType, TType>();
        }

        // Register type with instantiated implementation
        public void Register<TType>(object implementation)
        {
            _store.Add(typeof(TType), implementation);
        }
    }
}

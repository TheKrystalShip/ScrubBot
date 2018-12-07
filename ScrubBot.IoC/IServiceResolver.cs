using System;

namespace ScrubBot.IoC
{
    public interface IServiceResolver : IServiceProvider
    {
        void Register<TType>();
        void Register<TType, TImplementation>();
        void Register<TType>(object implementation);
        T Resolve<T>();
    }
}

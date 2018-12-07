using System;
using System.Collections.Generic;
using System.Reflection;

namespace ScrubBot.IoC
{
    public class DependencyInjector : IDependencyInjector
    {
        public IServiceResolver ServiceResolver { get; set; }

        public DependencyInjector() : this(new ServiceResolver())
        {

        }

        public DependencyInjector(IServiceResolver resolver)
        {
            ServiceResolver = resolver;
        }

        public virtual T GetInjectedInstance<T>() where T : class
        {
            return (T) GetInjectedInstance(typeof(T));
        }

        public virtual object GetInjectedInstance(Type type)
        {
            object toReturn = null;

            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                List<object> parameterValues = GetResolvedParameterValues(constructor);

                if (parameterValues.Count > 0)
                {
                    toReturn = Activator.CreateInstance(type, parameterValues.ToArray());
                }

                break;
            }

            // Null if parameterless constructor
            return toReturn ?? Activator.CreateInstance(type);
        }

        protected virtual List<object> GetResolvedParameterValues(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            List<object> parameterValues = new List<object>();

            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.ParameterType.IsInterface || parameter.ParameterType.IsAbstract || parameter.ParameterType.IsClass)
                {
                    Type parameterType = parameter.ParameterType;
                    object parameterValue = ServiceResolver.GetService(parameterType);
                    parameterValues.Add(parameterValue);
                }
                else
                {
                    throw new InvalidOperationException("Unable to inject a parameter that is not an interface, abstract type or class.");
                }
            }

            return parameterValues;
        }
    }
}

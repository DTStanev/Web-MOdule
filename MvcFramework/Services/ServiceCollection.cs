using MvcFramework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcFramework.Services
{
    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)this.CreateInstance(typeof(T)); throw new NotImplementedException();
        }

        public object CreateInstance(Type type)
        {
            if (this.dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }
            
            else if (type.IsInterface || type.IsAbstract)
            {
                throw new Exception($"Type {type.FullName} cannot be instantiated.");
            }

            var constructor = type.GetConstructors().First();
            var ctorParameters = constructor.GetParameters();
            var construcotrParameterObjects = new List<object>();

            foreach (var constructorParameter in ctorParameters)
            {
                var parameterObject = this.CreateInstance(constructorParameter.ParameterType);
                construcotrParameterObjects.Add(parameterObject);
            }

            var obj = constructor.Invoke(construcotrParameterObjects.ToArray());

            return obj;
        }
    }
}

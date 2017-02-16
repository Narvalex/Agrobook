using Agrobook.Core;
using System;
using System.Collections.Generic;

namespace Agrobook.Infrastructure.IoC
{
    public class SimpleContainer : ISimpleContainer
    {
        private IDictionary<Type, object> singletons = new Dictionary<Type, object>();
        private IDictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();

        public void Register<T>(Func<T> factory)
        {
            this.factories[typeof(T)] = () => factory.Invoke();
        }

        public void Register<T>(T instance)
        {
            this.singletons[typeof(T)] = instance;
        }

        public T ResolveNewOf<T>()
        {
            var type = typeof(T);
            if (!this.factories.ContainsKey(type))
                throw new FactoryMethodNotFoundException(type.Name);

            return (T)this.factories[type].Invoke();
        }

        public T ResolveSingleton<T>()
        {
            var type = typeof(T);
            if (!this.singletons.ContainsKey(type))
                throw new DependencyNotFoundException(type.Name);

            return (T)this.singletons[type];
        }
    }
}

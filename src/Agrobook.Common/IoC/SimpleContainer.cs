using Agrobook.Common;
using System;
using System.Collections.Generic;

namespace Agrobook.Common.IoC
{
    public class SimpleContainer : ISimpleContainer
    {
        private bool disposed = false;
        private IDictionary<Type, object> singletons = new Dictionary<Type, object>();
        private IDictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();

        public void Dispose()
        {
            if (this.disposed) return;
            this.disposed = true;
            this.singletons = null;
            this.factories = null;
        }

        public void Register<T>(Func<T> factory)
        {
            this.ThrowIfDisposed();
            this.factories[typeof(T)] = () => factory.Invoke();
        }

        public void Register<T>(T instance)
        {
            this.ThrowIfDisposed();
            this.singletons[typeof(T)] = instance;
        }

        public T ResolveNewOf<T>()
        {
            this.ThrowIfDisposed();
            var type = typeof(T);
            if (!this.factories.ContainsKey(type))
                throw new FactoryMethodNotFoundException(type.Name);

            return (T)this.factories[type].Invoke();
        }

        public T ResolveSingleton<T>()
        {
            this.ThrowIfDisposed();
            var type = typeof(T);
            if (!this.singletons.ContainsKey(type))
                throw new DependencyNotFoundException(type.Name);

            return (T)this.singletons[type];
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
                throw new ObjectDisposedException("SimpleContainer");
        }
    }
}

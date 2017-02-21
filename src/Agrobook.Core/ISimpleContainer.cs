using System;

namespace Agrobook.Core
{
    public interface ISimpleContainer
    {
        /// <summary>
        /// Resolves the unique instance of the specified object. If the the dependency is not 
        /// found then throws <see cref="DependencyNotFoundException"/>.
        /// </summary>
        T ResolveSingleton<T>();

        /// <summary>
        /// Resolves a new instance of T, as configured. If the factory method is 
        /// not found then throws <see cref="FactoryMethodNotFoundException"/>.
        /// </summary>
        T ResolveNewOf<T>();

        /// <summary>
        /// Registers an instance in the container as a singleton. If it already exists an instance of that type, 
        /// the old instance will be replaced.
        /// </summary>
        void Register<T>(T instance);

        /// <summary>
        /// Registers a factory method for the given type. If it already exists a factory method fot the given 
        /// type, the old factory method will be replaced.
        /// </summary>
        void Register<T>(Func<T> factory);
    }

    public class DependencyNotFoundException : Exception
    {
        public DependencyNotFoundException(string dependencyType)
            : base($"The dependency of type {dependencyType} could not be found in the simple container")
        { }
    }

    public class FactoryMethodNotFoundException : Exception
    {
        public FactoryMethodNotFoundException(string type)
            : base($"The factory method of type {type} was not found in the simple container")
        { }
    }
}

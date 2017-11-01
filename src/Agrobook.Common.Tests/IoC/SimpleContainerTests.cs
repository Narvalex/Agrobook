using Agrobook.Common;
using Agrobook.Common.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Common.Tests.IoC
{
    [TestClass]
    public class SimpleContainerTests
    {
        private readonly SimpleContainer container;

        public SimpleContainerTests()
        {
            this.container = new SimpleContainer();
        }

        [TestMethod]
        public void GivenNoSingletonRegistrationWhenResolvingThenThrows()
        {
            Assert.ThrowsException<DependencyNotFoundException>(() => this.container.ResolveSingleton<ITypeA>());
        }

        [TestMethod]
        public void GivenNoFactoryRegistrationWhenResolvingThenThrows()
        {
            Assert.ThrowsException<FactoryMethodNotFoundException>(() => this.container.ResolveNewOf<ITypeA>());
        }

        [TestMethod]
        public void WhenRegisteringInstanceThenCanResolve()
        {
            var instance = new TypeA1();
            this.container.Register<ITypeA>(instance);

            var resolvedInstance = this.container.ResolveSingleton<ITypeA>();

            Assert.AreEqual(instance, resolvedInstance);
        }

        [TestMethod]
        public void WhenRegisteringTwiceThenTheLastInstanceWins()
        {
            var instance1 = new TypeA1();
            this.container.Register<ITypeA>(instance1);

            var instance2 = new TypeA2();
            this.container.Register<ITypeA>(instance2);

            var resolved = this.container.ResolveSingleton<ITypeA>();

            Assert.AreNotEqual(instance1, resolved);
            Assert.AreEqual(instance2, resolved);
        }

        [TestMethod]
        public void CanRegisterMultipleInstances()
        {
            var a = new TypeA1();
            this.container.Register<ITypeA>(a);

            var b = new TypeB1();
            this.container.Register<ITypeB>(b);

            var resolvedA = this.container.ResolveSingleton<ITypeA>();
            var resolvedB = this.container.ResolveSingleton<ITypeB>();

            Assert.AreEqual(a, resolvedA);
            Assert.AreEqual(b, resolvedB);
        }

        [TestMethod]
        public void WhenRegisteringFactoryThenCanResolve()
        {
            var factoryMehtodWasCalled = false;
            this.container.Register<ITypeA>(() =>
            {
                factoryMehtodWasCalled = true;
                return new TypeA1();
            });

            Assert.IsFalse(factoryMehtodWasCalled);

            var instance = this.container.ResolveNewOf<ITypeA>();

            Assert.IsTrue(factoryMehtodWasCalled);
            Assert.IsTrue(instance is ITypeA);
        }

        [TestMethod]
        public void WhenRegisteringFactoryTwiceThenTheLastWins()
        {
            var factoryMehtod1WasCalled = false;
            this.container.Register<ITypeA>(() =>
            {
                factoryMehtod1WasCalled = true;
                return new TypeA1();
            });

            Assert.IsFalse(factoryMehtod1WasCalled);

            var factoryMehtod2WasCalled = false;
            this.container.Register<ITypeA>(() =>
            {
                factoryMehtod2WasCalled = true;
                return new TypeA2();
            });

            Assert.IsFalse(factoryMehtod2WasCalled);

            var instance = this.container.ResolveNewOf<ITypeA>();

            Assert.IsFalse(factoryMehtod1WasCalled);
            Assert.IsTrue(factoryMehtod2WasCalled);
            Assert.IsTrue(instance is ITypeA);
            Assert.IsTrue(instance is TypeA2);
        }

        [TestMethod]
        public void CanRegisterMultipleFactoryTypes()
        {
            this.container.Register<ITypeA>(() => new TypeA1());
            this.container.Register<ITypeB>(() => new TypeB1());

            Assert.IsTrue(this.container.ResolveNewOf<ITypeA>() != null);
            Assert.IsTrue(this.container.ResolveNewOf<ITypeB>() != null);
        }

        [TestMethod]
        public void WhenResolvingNewInstancesTheyAreAllNewOnes()
        {
            this.container.Register<ITypeA>(() => new TypeA1());

            var i1 = this.container.ResolveNewOf<ITypeA>();
            var i2 = this.container.ResolveNewOf<ITypeA>();

            Assert.IsTrue(i1 is ITypeA);
            Assert.IsTrue(i2 is ITypeA);
            Assert.AreNotEqual(i2, i1);
        }
    }

    public interface ITypeA { }

    public interface ITypeB { }

    public class TypeA1 : ITypeA { }
    public class TypeA2 : ITypeA { }

    public class TypeB1 : ITypeB { }
    public class TypeB2 : ITypeB { }
}

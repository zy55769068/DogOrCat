using Autofac;

namespace DogOrCat.Framework
{
    public class Resolver
    {
        private static IContainer container;

        public static void Initialize(IContainer Container)
        {
            Resolver.container = Container;
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
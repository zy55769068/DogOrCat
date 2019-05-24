using Autofac;

namespace DogOrCat.Framework
{
    public class Resolver
    {
        private static IContainer container;

        public static void Initialize(IContainer Container)
        {
            container = Container;
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
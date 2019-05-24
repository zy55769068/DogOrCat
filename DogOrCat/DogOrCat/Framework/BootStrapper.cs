using System.Linq;
using System.Reflection;
using Autofac;
using DogOrCat.ViewModels;
using Xamarin.Forms;

namespace DogOrCat.Framework
{
    public class BootStrapper
    {
        public BootStrapper()
        {
            Initialize();
            FinishInitialize();
        }

        protected ContainerBuilder ContainerBuilder { get; private set; }

        protected virtual void Initialize()
        {
            ContainerBuilder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();

            foreach (var typeInfo in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page))))
                ContainerBuilder.RegisterType(typeInfo.AsType());

            foreach (var typeInfo in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(ViewModel))))
                ContainerBuilder.RegisterType(typeInfo.AsType());
        }

        private void FinishInitialize()
        {
            var container = ContainerBuilder.Build();
            Resolver.Initialize(container);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using DogOrCat.ViewModels;
using Xamarin.Forms;

namespace DogOrCat.Framework
{
    public class BootStrapper
    {
        protected ContainerBuilder ContainerBuilder { get;private set; }

        public BootStrapper()
        {
            Initialize();
            FinishInitialize();

        }

        protected virtual void Initialize()
        {
            ContainerBuilder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();

            foreach (var typeInfo in currentAssembly.DefinedTypes.Where(e=>e.IsSubclassOf(typeof(Page))))
            {
                ContainerBuilder.RegisterType(typeInfo.AsType());
            }

            foreach (var typeInfo in currentAssembly.DefinedTypes.Where(e=>e.IsSubclassOf(typeof(ViewModel))))
            {
                ContainerBuilder.RegisterType(typeInfo.AsType());
            }
        }

        private void FinishInitialize()
        {
            var container = ContainerBuilder.Build();
            Resolver.Initialize(container);
        }

    }
}

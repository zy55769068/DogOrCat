using Autofac;
using DogOrCat.Interfaces;
using DogOrCat.UWP.WindowsML;

namespace DogOrCat.UWP
{
    public class BootStrapper : Framework.BootStrapper
    {
        public static void Init()
        {
            var instance = new BootStrapper();
        }

        protected override void Initialize()
        {
            base.Initialize();

            ContainerBuilder.RegisterType<OnnxClassification>().As<IClassification>().SingleInstance();
        }
    }
}
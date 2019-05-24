using Autofac;
using DogOrCat.Interfaces;
using DogOrCat.iOS.CoreML;

namespace DogOrCat.iOS
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

            ContainerBuilder.RegisterType<CoreMLClassification>().As<IClassification>();
        }
    }
}
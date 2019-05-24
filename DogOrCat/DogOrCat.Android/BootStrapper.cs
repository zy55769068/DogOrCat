using Autofac;
using DogOrCat.Droid.TensorFlowClassfication;
using DogOrCat.Interfaces;

namespace DogOrNot.Droid
{
    public class BootStrapper: DogOrCat.Framework.BootStrapper
    {
        public static void Init()
        {
            var instance = new BootStrapper();
        }
        protected override void Initialize()
        {
            base.Initialize();

            ContainerBuilder.RegisterType<TensorFlowClassification>().As<IClassification>().SingleInstance();
        }
    }
}
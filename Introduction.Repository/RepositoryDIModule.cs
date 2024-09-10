using Autofac;
using Introduction.Repository.Common;

namespace Introduction.Repository
{
    public class RepositoryDIModule: Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CarRepository>().As<ICarRepository>();
            containerBuilder.RegisterType<CarTypeRepository>().As<ICarTypeRepository>();
        }
    }
}

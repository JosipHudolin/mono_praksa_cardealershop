using Autofac;
using Introduction.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Service
{
    public class ServiceDIModule: Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CarService>().As<ICarService>();
            containerBuilder.RegisterType<CarTypeService>().As<ICarTypeService>();
        }
    }
}

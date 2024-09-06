using Introduction.Model;
using Introduction.Repository;
using Introduction.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Service
{
    public class CarTypeService: ICarTypeService
    {
        public List<CarType> Get()
        {
            var repository = new CarTypeRepository();
            if (repository.Get() != null && repository.Get().Count > 0)
            {
                return repository.Get();
            }
            else
            {
                return null;
            }
        }

        public CarType GetById(Guid id)
        {
            var repository = new CarTypeRepository();
            if (repository.GetById(id) != null)
            {
                return repository.GetById(id);
            }
            else
            {
                return null;
            }
        }

        public bool InputCarType(CarType carType)
        {
            var repository = new CarTypeRepository();
            return repository.InputCarType(carType);
        }

        public bool UpdateNameById(Guid id, string name)
        {
            var repository = new CarTypeRepository();
            return repository.UpdateNameById(id, name);
        }

        public bool Delete(Guid id)
        {
            var repository = new CarTypeRepository();
            return repository.Delete(id);
        }
    }
}

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
        public async Task<List<CarType>> Get()
        {
            var repository = new CarTypeRepository();
            var currentCarTypes = await repository.Get();
            if (currentCarTypes != null && currentCarTypes.Count > 0)
            {
                return currentCarTypes;
            }
            else
            {
                return null;
            }
        }

        public async Task<CarType> GetById(Guid id)
        {
            var repository = new CarTypeRepository();
            var currentCarType = await repository.GetById(id);
            if (currentCarType != null)
            {
                return currentCarType;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InputCarType(CarType carType)
        {
            var repository = new CarTypeRepository();
            return await repository.InputCarType(carType);
        }

        public async Task<bool> UpdateNameById(Guid id, string name)
        {
            var repository = new CarTypeRepository();
            return await repository.UpdateNameById(id, name);
        }

        public async Task<bool> Delete(Guid id)
        {
            var repository = new CarTypeRepository();
            return await repository.Delete(id);
        }
    }
}

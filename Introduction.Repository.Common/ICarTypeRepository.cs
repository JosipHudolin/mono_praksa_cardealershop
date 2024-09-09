using Introduction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Repository.Common
{
    public interface ICarTypeRepository
    {
        Task<List<CarType>> Get();
        Task<CarType> GetById(Guid id);
        Task<bool> InputCarType(CarType carType);
        Task<bool> UpdateNameById(Guid id, string name);
        Task<bool> Delete(Guid id);
    }
}

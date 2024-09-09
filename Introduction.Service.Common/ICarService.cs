using Introduction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarById(Guid id);
        Task<List<Car>> GetAllCars();
        Task<bool> InputCar(Car car);
        Task<bool> UpdateCarMileage(Guid id, CarUpdate car);
        Task<bool> UpdateCarDescription(CarUpdate car, Guid id);
        Task<bool> UpdateCar(CarUpdate car, Guid id);
        Task<bool> DeleteCar(Guid id);
    }
}

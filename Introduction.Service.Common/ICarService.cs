using Introduction.Common;
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarByIdAsync(Guid id);
        Task<List<Car>> GetAllCarsAsync(AddFilter filter, Paging paging, Sorting sorting);
        Task<bool> InputCarAsync(Car car);
        Task<bool> UpdateCarAsync(CarUpdate car, Guid id);
        Task<bool> DeleteCarAsync(Guid id);
    }
}

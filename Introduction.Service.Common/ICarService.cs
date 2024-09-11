using Introduction.Common;
using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarByIdAsync(Guid id);
        Task<List<Car>> GetAllCarsAsync(AddFilter filter, Paging paging, Sorting sorting);
        Task<bool> InputCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(Guid id);
    }
}

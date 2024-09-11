using Introduction.Common;
using Introduction.Model;


namespace Introduction.Repository.Common
{
    public interface ICarRepository
    {
        Task<Car> GetCarByIdAsync(Guid id);
        Task<List<Car>> GetAllCarsAsync(AddFilter filter, Paging paging, Sorting sorting);
        Task<bool> InputCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(Guid id);
    }
}

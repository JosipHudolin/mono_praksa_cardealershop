using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICarTypeRepository
    {
        Task<List<CarType>> GetAsync();
        Task<CarType> GetByIdAsync(Guid id);
        Task<List<Car>> GetCarsAsync(Guid id);
        Task<bool> InputCarTypeAsync(CarType carType);
        Task<bool> UpdateNameByIdAsync(Guid id, string name);
        Task<bool> DeleteAsync(Guid id);
    }
}

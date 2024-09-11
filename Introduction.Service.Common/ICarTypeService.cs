using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICarTypeService
    {
        Task<List<CarType>> GetAsync();
        Task<CarType> GetByIdAsync(Guid id);
        Task<List<Car>> GetCarsAsync(Guid id);
        Task<bool> InputCarTypeAsync(CarType carType);
        Task<bool> UpdateNameByIdAsync(Guid id, string name);
        Task<bool> DeleteAsync(Guid id);
    }
}

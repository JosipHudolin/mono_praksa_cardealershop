using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICarTypeRepository
    {
        Task<List<CarType>> Get();
        Task<CarType> GetById(Guid id);
        Task<List<Car>> GetCars(Guid id);
        Task<bool> InputCarType(CarType carType);
        Task<bool> UpdateNameById(Guid id, string name);
        Task<bool> Delete(Guid id);
    }
}

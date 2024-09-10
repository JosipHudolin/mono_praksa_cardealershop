using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICarService
    {
        Task<Car> GetCarById(Guid id);
        Task<List<Car>> GetAllCars();
        Task<bool> InputCar(Car car);
        Task<bool> UpdateCar(CarUpdate car, Guid id);
        Task<bool> DeleteCar(Guid id);
    }
}

using Introduction.Model;
using Introduction.Repository.Common;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CarService: ICarService
    {

        ICarRepository _carRepository;
        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<Car> GetCarById(Guid id)
        {
            var currentCar = _carRepository.GetCarById(id);
            return await currentCar;
        }

        public async Task<List<Car>> GetAllCars()
        {
            var currentCars = await _carRepository.GetAllCars();
            if (currentCars != null && currentCars.Count > 0)
            {
                return currentCars;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InputCar(Car car)
        {
            return await _carRepository.InputCar(car);
        }

        public async Task<bool> UpdateCar(CarUpdate car, Guid id)
        {
            return await _carRepository.UpdateCar(car, id);
        }

        public async Task<bool> DeleteCar(Guid id)
        {
            return await _carRepository.DeleteCar(id);
        }
    }
}

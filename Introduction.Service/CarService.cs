using Introduction.Common;
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

        public async Task<Car> GetCarByIdAsync(Guid id)
        {
            var currentCar = _carRepository.GetCarByIdAsync(id);
            return await currentCar;
        }

        public async Task<List<Car>> GetAllCarsAsync(AddFilter filter, Paging paging, Sorting sorting)
        {
            var currentCars = await _carRepository.GetAllCarsAsync(filter, paging, sorting);
            if (currentCars != null && currentCars.Count > 0)
            {
                return currentCars;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InputCarAsync(Car carCreate)
        {
            return await _carRepository.InputCarAsync(carCreate);
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            return await _carRepository.UpdateCarAsync(car);
        }

        public async Task<bool> DeleteCarAsync(Guid id)
        {
            return await _carRepository.DeleteCarAsync(id);
        }
    }
}

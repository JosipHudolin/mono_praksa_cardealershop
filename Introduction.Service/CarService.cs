using Introduction.Model;
using Introduction.Repository;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CarService: ICarService
    {
        public async Task<Car> GetCarById(Guid id)
        {
            var repository = new CarRepository();
            var currentCar = repository.GetCarById(id);
            if (currentCar != null)
            {
                return await currentCar;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Car>> GetAllCars()
        {
            var repository = new CarRepository();
            var currentCars = await repository.GetAllCars();
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
            var repository = new CarRepository();
            return await repository.InputCar(car);
        }

        public async Task<bool> UpdateCarMileage(Guid id, CarUpdate car)
        {
            var repository = new CarRepository();
            return await repository.UpdateCarMileage(id, car);
        }

        public async Task<bool> UpdateCarDescription(CarUpdate car, Guid id)
        {
            var repository = new CarRepository();
            return await repository.UpdateCarDescription(car, id);
        }

        public async Task<bool> UpdateCar(CarUpdate car, Guid id)
        {
            var repository = new CarRepository();
            return await repository.UpdateCar(car, id);
        }

        public async Task<bool> DeleteCar(Guid id)
        {
            var repository = new CarRepository();
            return await repository.DeleteCar(id);
        }
    }
}

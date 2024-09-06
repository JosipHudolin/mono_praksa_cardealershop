using Introduction.Model;
using Introduction.Repository;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CarService: ICarService
    {
        public Car GetCarById(Guid id)
        {
            var repository = new CarRepository();
            if (repository.GetCarById(id) != null)
            {
                return repository.GetCarById(id);
            }
            else
            {
                return null;
            }
        }

        public List<Car> GetAllCars()
        {
            var repository = new CarRepository();
            if (repository.GetAllCars() != null && repository.GetAllCars().Count > 0)
            {
                return repository.GetAllCars();
            }
            else
            {
                return null;
            }
        }

        public bool InputCar(Car car)
        {
            var repository = new CarRepository();
            return repository.InputCar(car);
        }

        public bool UpdateCarMileage(Guid id, CarUpdate car)
        {
            var repository = new CarRepository();
            return repository.UpdateCarMileage(id, car);
        }

        public bool UpdateCarDescription(CarUpdate car, Guid id)
        {
            var repository = new CarRepository();
            return repository.UpdateCarDescription(car, id);
        }

        public bool UpdateCar(CarUpdate car, Guid id)
        {
            var repository = new CarRepository();
            return repository.UpdateCar(car, id);
        }

        public bool DeleteCar(Guid id)
        {
            var repository = new CarRepository();
            return repository.DeleteCar(id);
        }
    }
}

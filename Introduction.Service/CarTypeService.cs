using Introduction.Model;
using Introduction.Repository.Common;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class CarTypeService: ICarTypeService
    {
        ICarTypeRepository _carTypeRepository;

        public CarTypeService(ICarTypeRepository carTypeRepository)
        {
            _carTypeRepository = carTypeRepository;
        }

        public async Task<List<CarType>> GetAsync()
        {
            var currentCarTypes = await _carTypeRepository.GetAsync();
            if (currentCarTypes != null && currentCarTypes.Count > 0)
            {
                return currentCarTypes;
            }
            else
            {
                return null;
            }
        }

        public async Task<CarType> GetByIdAsync(Guid id)
        {
            var currentCarType = await _carTypeRepository.GetByIdAsync(id);
            if (currentCarType != null)
            {
                return currentCarType;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Car>> GetCarsAsync(Guid id)
        {
            var currentCarTypes = await _carTypeRepository.GetCarsAsync(id);
            if (currentCarTypes != null && currentCarTypes.Count > 0)
            {
                return currentCarTypes;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InputCarTypeAsync(CarType carType)
        {
            return await _carTypeRepository.InputCarTypeAsync(carType);
        }

        public async Task<bool> UpdateNameByIdAsync(Guid id, string name)
        {
            return await _carTypeRepository.UpdateNameByIdAsync(id, name);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _carTypeRepository.DeleteAsync(id);
        }
    }
}

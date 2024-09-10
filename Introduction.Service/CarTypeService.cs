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

        public async Task<List<CarType>> Get()
        {
            var currentCarTypes = await _carTypeRepository.Get();
            if (currentCarTypes != null && currentCarTypes.Count > 0)
            {
                return currentCarTypes;
            }
            else
            {
                return null;
            }
        }

        public async Task<CarType> GetById(Guid id)
        {
            var currentCarType = await _carTypeRepository.GetById(id);
            if (currentCarType != null)
            {
                return currentCarType;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InputCarType(CarType carType)
        {
            return await _carTypeRepository.InputCarType(carType);
        }

        public async Task<bool> UpdateNameById(Guid id, string name)
        {
            return await _carTypeRepository.UpdateNameById(id, name);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _carTypeRepository.Delete(id);
        }
    }
}

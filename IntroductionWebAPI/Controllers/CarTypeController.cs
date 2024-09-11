using Introduction.Model;
using Introduction.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        ICarTypeService _carTypeService;

        public CarTypeController(ICarTypeService carTypeService)
        {
            _carTypeService = carTypeService;
        }

        [HttpGet]
        [Route("getCarTypes")]
        public async Task<IActionResult> GetAsync()
        {
            var currentCarTypes = await _carTypeService.GetAsync();
            if (currentCarTypes != null)
            {
                return Ok(currentCarTypes);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("getCars")]
        public async Task<IActionResult> GetCarsAsync(Guid id)
        {
            var currentCars = await _carTypeService.GetCarsAsync(id);
            if (currentCars != null)
            {
                return Ok(currentCars);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCarTypeName/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var currentCar = await _carTypeService.GetByIdAsync(id);
            if (currentCar != null)
            {
                return Ok(currentCar);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("insertCarType")]
        public async Task<IActionResult> InputCarTypeAsync([FromBody] CarType carType)
        {
            if (await _carTypeService.InputCarTypeAsync(carType))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarTypeName/{id}")]
        public async Task<IActionResult> UpdateNameByIdAsync(Guid id, [FromBody] string name)
        {
            if (await _carTypeService.UpdateNameByIdAsync(id, name))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }

        [HttpDelete]
        [Route("deleteCarType/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (await _carTypeService.DeleteAsync(id))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }
    }
}

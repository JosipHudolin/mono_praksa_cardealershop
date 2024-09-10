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
        [Route("getCarTypeName")]
        public async Task<IActionResult> Get()
        {
            var currentCarTypes = await _carTypeService.Get();
            if (currentCarTypes != null)
            {
                return Ok(currentCarTypes);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCarTypeName/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currentCar = await _carTypeService.GetById(id);
            if (currentCar != null)
            {
                return Ok(currentCar);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("insertCarType")]
        public async Task<IActionResult> InputCarType([FromBody] CarType carType)
        {
            if (await _carTypeService.InputCarType(carType))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarTypeName/{id}")]
        public async Task<IActionResult> UpdateNameById(Guid id, [FromBody] string name)
        {
            if (await _carTypeService.UpdateNameById(id, name))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }

        [HttpDelete]
        [Route("deleteCarType/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _carTypeService.Delete(id))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }
    }
}

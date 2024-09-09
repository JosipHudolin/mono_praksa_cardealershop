using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        [HttpGet]
        [Route("getCarTypeName")]
        public async Task<IActionResult> Get()
        {
            var service = new CarTypeService();
            var currentCarTypes = await service.Get();
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
            var service = new CarTypeService();
            var currentCar = await service.GetById(id);
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
            var service = new CarTypeService();
            if (await service.InputCarType(carType))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarTypeName/{id}")]
        public async Task<IActionResult> UpdateNameById(Guid id, [FromBody] string name)
        {
            var service = new CarTypeService();
            if (await service.UpdateNameById(id, name))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }

        [HttpDelete]
        [Route("deleteCarType/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var service = new CarTypeService();
            if (await service.Delete(id))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }
    }
}

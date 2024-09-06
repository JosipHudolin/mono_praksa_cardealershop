using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        [HttpGet]
        [Route("getCarTypeName")]
        public IActionResult Get()
        {
            var service = new CarTypeService();
            if (service.Get() != null)
            {
                return Ok(service.Get());
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCarTypeName/{id}")]
        public IActionResult GetById(Guid id)
        {
            var service = new CarTypeService();
            if (service.GetById(id) != null)
            {
                return Ok(service.GetById(id));
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("insertCarType")]
        public IActionResult InputCarType([FromBody] CarType carType)
        {
            var service = new CarTypeService();
            if (service.InputCarType(carType))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarTypeName/{id}")]
        public IActionResult UpdateNameById(Guid id, [FromBody] string name)
        {
            var service = new CarTypeService();
            if (service.UpdateNameById(id, name))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }

        [HttpDelete]
        [Route("deleteCarType/{id}")]
        public IActionResult Delete(Guid id)
        {
            var service = new CarTypeService();
            if (service.Delete(id))
            {
                return Ok();
            }
            return BadRequest("Car type not found!");
        }
    }
}

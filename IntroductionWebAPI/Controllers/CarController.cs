using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        [HttpGet]
        [Route("getCars")]
        public IActionResult GetAllCars()
        {
            var service = new CarService();
            if (service.GetAllCars() != null)
            {
                return Ok(service.GetAllCars());
            }
            return BadRequest();
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public IActionResult GetCarById(Guid id)
        {
            var service = new CarService();
            if (service.GetCarById(id) != null)
            {
                return Ok(service.GetCarById(id));
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("inputCar")]
        public IActionResult InputCar([FromBody] Car car)
        {
            var service = new CarService();
            if (service.InputCar(car))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarMileage/{id}")]
        public IActionResult UpdateCarMileage(Guid id, [FromBody] CarUpdate car )
        {
            var service = new CarService();
            if (service.UpdateCarMileage(id, car))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpPut]
        [Route("updateCarDescription/{id}")]
        public IActionResult UpdateCarDescription([FromBody] CarUpdate car, Guid id)
        {
            var service = new CarService();
            if (service.UpdateCarDescription(car, id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public IActionResult UpdateCar([FromBody] CarUpdate car, Guid id)
        {
            var service = new CarService();
            if (service.UpdateCar(car, id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpDelete]
        [Route("deleteCar/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCar(Guid id)
        {
            var service = new CarService();
            if (service.DeleteCar(id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }
    }
}

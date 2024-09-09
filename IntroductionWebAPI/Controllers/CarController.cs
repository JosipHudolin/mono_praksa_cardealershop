using Introduction.Model;
using Introduction.Service;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        [HttpGet]
        [Route("getCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var service = new CarService();
            var currentCar = await service.GetAllCars();
            if (currentCar != null)
            {
                return Ok(currentCar);
            }
            return BadRequest();
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public async Task<IActionResult> GetCarById(Guid id)
        {
            var service = new CarService();
            var currentCar = await service.GetCarById(id);
            if (currentCar != null)
            {
                return Ok(currentCar);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("inputCar")]
        public async Task<IActionResult> InputCar([FromBody] Car car)
        {
            var service = new CarService();
            if (await service.InputCar(car))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCarMileage/{id}")]
        public async Task<IActionResult> UpdateCarMileage(Guid id, [FromBody] CarUpdate car )
        {
            var service = new CarService();
            if (await service.UpdateCarMileage(id, car))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpPut]
        [Route("updateCarDescription/{id}")]
        public async Task<IActionResult> UpdateCarDescription([FromBody] CarUpdate car, Guid id)
        {
            var service = new CarService();
            if (await service.UpdateCarDescription(car, id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarUpdate car, Guid id)
        {
            var service = new CarService();
            if (await service.UpdateCar(car, id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpDelete]
        [Route("deleteCar/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var service = new CarService();
            if (await service.DeleteCar(id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }
    }
}

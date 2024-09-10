using Introduction.Model;
using Introduction.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [Route("getCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var currentCar = await _carService.GetAllCars();
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
            var currentCar = await _carService.GetCarById(id);
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
            if (await _carService.InputCar(car))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public async Task<IActionResult> UpdateCar([FromBody] CarUpdate car, Guid id)
        {
            if (await _carService.UpdateCar(car, id))
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
            if (await _carService.DeleteCar(id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }
    }
}

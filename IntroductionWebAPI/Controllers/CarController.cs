using Introduction.Common;
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
        public async Task<IActionResult> GetAllCarsAsync([FromQuery] string make = "", [FromQuery] string model = "", [FromQuery] int yearFrom = 0, [FromQuery] int yearTo = 0, [FromQuery] int mileageFrom = 0, [FromQuery] int mileageTo = 0, [FromQuery] Guid? carTypeId = null, [FromQuery] string searchQuery = "", [FromQuery] string orderBy ="Make", [FromQuery] string sortDirection = "DESC", [FromQuery] int rpp = 5, [FromQuery] int pageNumber = 1)
        {
            AddFilter filter = new AddFilter();
            filter.Model = model;
            filter.Make = make;
            filter.YearFrom = yearFrom;
            filter.YearTo = yearTo;
            filter.MileageFrom = mileageFrom;
            filter.MileageTo = mileageTo;
            filter.SearchQuery = searchQuery;
            filter.CarTypeId = carTypeId;

            Sorting sorting = new Sorting();
            sorting.SortDirection = sortDirection;
            sorting.OrderBy = orderBy;

            Paging paging = new Paging();
            paging.PageNumber = pageNumber;
            paging.Rpp = rpp;
            var currentCars = await _carService.GetAllCarsAsync(filter, paging, sorting);
            if (currentCars != null)
            {
                return Ok(currentCars);
            }
            return Ok("There are no matching cars!");
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public async Task<IActionResult> GetCarByIdAsync(Guid id)
        {
            var currentCar = await _carService.GetCarByIdAsync(id);
            if (currentCar != null)
            {
                return Ok(currentCar);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("inputCar")]
        public async Task<IActionResult> InputCarAsync([FromBody] Car car)
        {
            if (await _carService.InputCarAsync(car))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public async Task<IActionResult> UpdateCarAsync([FromBody] CarUpdate car, Guid id)
        {
            if (await _carService.UpdateCarAsync(car, id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }

        [HttpDelete]
        [Route("deleteCar/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCarAsync(Guid id)
        {
            if (await _carService.DeleteCarAsync(id))
            {
                return Ok();
            }
            return BadRequest("Car not found!");
        }
    }
}

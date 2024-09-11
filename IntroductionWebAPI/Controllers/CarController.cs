using Introduction.Common;
using Introduction.Service.Common;
using Microsoft.AspNetCore.Mvc;
using IntroductionWebAPI.RestModels;
using Introduction.Model;
using IntroductionWebAPI.Models;

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
            List<CarGet> cars = new List<CarGet>();
            foreach (var car in currentCars)
            {
                var getCar = new CarGet();
                getCar.Make = car.Make;
                getCar.Model = car.Model;
                getCar.Year = car.Year;
                getCar.CarTypeName = car.CarType.Name;
                getCar.Mileage = car.Mileage;
                cars.Add(getCar);
            }
            if (cars != null)
            {
                return Ok(cars);
            }
            return Ok("There are no matching cars!");
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public async Task<IActionResult> GetCarByIdAsync(Guid id)
        {
            var currentCar = await _carService.GetCarByIdAsync(id);
            CarGet car = new CarGet();
            car.Make = currentCar.Make;
            car.Model = currentCar.Model;
            car.Year = currentCar.Year;
            car.CarTypeName = currentCar.CarType.Name;
            car.Mileage = currentCar.Mileage;
            if (car != null)
            {
                return Ok(car);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("inputCar")]
        public async Task<IActionResult> InputCarAsync([FromBody] CarCreate carCreate)
        {
            Car car = new Car();
            car.Make = carCreate.Make;
            car.Model = carCreate.Model;
            car.CarTypeId = carCreate.CarTypeId;
            car.Year = carCreate.Year;
            car.Description = carCreate.Description;
            car.Mileage = carCreate.Mileage;
            if (await _carService.InputCarAsync(car))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public async Task<IActionResult> UpdateCarAsync(Guid id, [FromBody] CarUpdate car)
        {
            Car updatedCar = new Car();
            updatedCar.Id = id;
            updatedCar.Mileage = car.Mileage;
            updatedCar.Description = car.Description;
            if (await _carService.UpdateCarAsync(updatedCar))
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

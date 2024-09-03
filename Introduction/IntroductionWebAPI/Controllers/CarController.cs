using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public static List<Car> Cars { get; set; } = new List<Car>();

        [HttpGet]
        [Route("getCars")]
        public IActionResult GetCars(string? make, string? model, int? year, int? mileage, DateOnly? inputDate, string? beforeAfter)
        {
            List<Car> selectedCars = Cars;
            if (make != null)
            {
                selectedCars = selectedCars.Where(car => string.Equals(car.Make, make, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (model != null)
            {
                selectedCars = selectedCars.Where(car => string.Equals(car.Model, model, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (year != null)
            {
                selectedCars = selectedCars.Where(car => car.Year == year).ToList();
            }
            if (mileage != null)
            {
                selectedCars = selectedCars.Where(car => car.Mileage == mileage).ToList();
            }
            if (inputDate != DateOnly.MinValue && string.Equals(beforeAfter, "after", StringComparison.OrdinalIgnoreCase))
            {
                selectedCars = selectedCars.Where(car => car.InputDate > inputDate).ToList();
            }
            else if (inputDate != DateOnly.MinValue && string.Equals(beforeAfter, "before", StringComparison.OrdinalIgnoreCase))
            {
                selectedCars = selectedCars.Where(car => car.InputDate < inputDate).ToList();
            }
            return Ok(selectedCars);
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public IActionResult GetCarById(int id)
        {
            Car selectedCar = null;
            foreach (Car car in Cars)
            {
                if (car.Id == id)
                {
                    selectedCar = car;
                }
            }
            return Ok(selectedCar);
        }

        [HttpPost]
        [Route("inputCar")]
        public IActionResult InputCar([FromBody] Car car)
        {
            bool exists = Cars.Any(x => x.Id == car.Id);
            if (exists)
            {
                return BadRequest();
            }
            car.InputDate = DateOnly.FromDateTime(DateTime.Now);
            Cars.Add(car);
            return Ok();
        }

        [HttpPut]
        [Route("updateCarMileage")]
        public IActionResult UpdateCarMileage([FromBody] Car car)
        {
            bool exists = Cars.Any(x => x.Id == car.Id);
            if (!exists)
            {
                return BadRequest();
            }
            foreach (Car vehicle in Cars)
            {
                if (car.Id == vehicle.Id)
                {
                    vehicle.Mileage = car.Mileage;
                }
            }
            return Ok();
        }

        [HttpPut]
        [Route("updateCarDescription")]
        public IActionResult UpdateCarDescription([FromBody] Car car)
        {
            bool exists = Cars.Any(x => x.Id == car.Id);
            if (!exists)
            {
                return BadRequest();
            }
            foreach (Car vehicle in Cars)
            {
                if (car.Id == vehicle.Id)
                {
                    vehicle.Decription = car.Decription;
                }
            }
            return Ok();
        }

        [HttpDelete]
        [Route("deleteCar/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCar(int id)
        {
            for (int i = 0; i < Cars.Count; i++)
            {
                if (id == Cars[i].Id)
                {
                    Cars.RemoveAt(i);
                }
            }
            return Ok();
        }
    }
}

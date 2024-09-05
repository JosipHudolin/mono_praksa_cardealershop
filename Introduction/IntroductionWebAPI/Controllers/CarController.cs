using IntroductionWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=postgres;Database=car-dealershop";

        [HttpGet]
        [Route("getCars")]
        public IActionResult GetAllCars()
        {
            try
            {
                List<Car> cars = new List<Car>();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                var commandText = @"SELECT c.""Id"", c.""Make"", c.""Model"", c.""Year"", c.""Mileage"", c.""Description"", c.""InputDate"", 
                                   ct.""Id"" as ""CarTypeId"", ct.""Name"" as ""CarTypeName""
                            FROM ""Car"" c
                            LEFT JOIN ""CarType"" ct ON c.""CarTypeId"" = ct.""Id"";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var car = new Car
                        {
                            Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString()),
                            Make = reader.IsDBNull(1) ? null : reader[1].ToString(),
                            Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString(),
                            CarType = reader.IsDBNull(reader.GetOrdinal("CarTypeId")) ? null : new CarType
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("CarTypeId")),
                                Name = reader.IsDBNull(reader.GetOrdinal("CarTypeName")) ? null : reader.GetString(reader.GetOrdinal("CarTypeName"))
                            },
                            Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : reader.GetInt32(reader.GetOrdinal("Year")),
                            Mileage = reader.IsDBNull(reader.GetOrdinal("Mileage")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mileage")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            InputDate = reader.IsDBNull(reader.GetOrdinal("InputDate")) ? (DateOnly?)null : reader.GetFieldValue<DateOnly?>(reader.GetOrdinal("InputDate"))
                        };

                        cars.Add(car);
                    }
                }
                connection.Close();

                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("getCarById/{id}")]
        public IActionResult GetCarById(Guid id)
        {
            try
            {
                var car = new Car();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                string commandText = "SELECT c.\"Id\", c.\"Make\", c.\"Model\", c.\"Year\", c.\"Mileage\", c.\"Description\", c.\"InputDate\", \r\n       ct.\"Id\" as \"CarTypeId\", ct.\"Name\" as \"CarTypeName\"\r\nFROM \"Car\" c\r\nLEFT JOIN \"CarType\" ct ON c.\"CarTypeId\" = ct.\"Id\"\r\nWHERE c.\"Id\" = @id;\r\n";
                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    car.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    car.Make = reader.IsDBNull(1) ? null : reader[1].ToString();
                    car.Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString();
                    car.CarType = reader.IsDBNull(reader.GetOrdinal("CarTypeId")) ? null : new CarType
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CarTypeId")),
                        Name = reader.IsDBNull(reader.GetOrdinal("CarTypeName")) ? null : reader.GetString(reader.GetOrdinal("CarTypeName"))
                    };
                    car.Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : reader.GetInt32(reader.GetOrdinal("Year"));
                    car.Mileage = reader.IsDBNull(reader.GetOrdinal("Mileage")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mileage"));
                    car.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));
                    car.InputDate = reader.IsDBNull(reader.GetOrdinal("InputDate")) ? (DateOnly?)null : reader.GetFieldValue<DateOnly?>(reader.GetOrdinal("InputDate"));
                }
                if (car == null)
                {
                    return NotFound();
                }
                connection.Close();
                return Ok(car);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("inputCar")]
        public IActionResult InputCar([FromBody] Car car)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "INSERT INTO \"Car\" (\"Id\", \"Make\", \"Model\", \"CarTypeId\", \"Year\", \"Mileage\", \"Description\", \"InputDate\") VALUES (@id, @make, @model, @carTypeId, @year, @mileage, @description, @inputDate)";

                connection.Open();

                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@make", car.Make);
                command.Parameters.AddWithValue("@model", car.Model);
                command.Parameters.AddWithValue("@carTypeId", NpgsqlTypes.NpgsqlDbType.Uuid, car.CarType.Id);
                command.Parameters.AddWithValue("@year", car.Year);
                command.Parameters.AddWithValue("@mileage", car.Mileage);
                command.Parameters.AddWithValue("@description", car.Description);
                command.Parameters.AddWithValue("@inputDate", NpgsqlTypes.NpgsqlDbType.Date, DateOnly.FromDateTime(DateTime.Now));

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return BadRequest();
                }

                connection.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateCarMileage/{id}")]
        public IActionResult UpdateCarMileage(Guid id, [FromBody] CarUpdate car )
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "UPDATE \"Car\" SET \"Mileage\" = @mileage WHERE \"Id\" = @id;";

                connection.Open();

                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@mileage", car.Mileage);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return BadRequest("Not found!");
                }
                connection.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("updateCarDescription/{id}")]
        public IActionResult UpdateCarDescription([FromBody] CarUpdate car, Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = "UPDATE \"Car\" SET \"Description\" = @description WHERE \"Id\" = @id;";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@description", car.Description);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return NotFound("Car not found!");
                }
                connection.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("updateCar/{id}")]
        public IActionResult UpdateCar([FromBody] CarUpdate car, Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = "UPDATE \"Car\" SET \"Description\" = @description \"Mileage\" = @mileage WHERE \"Id\" = @id;";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@description", car.Description);
                command.Parameters.AddWithValue("@mileage", car.Mileage);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return NotFound("Car not found!");
                }
                connection.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("deleteCar/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCar(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "DELETE FROM \"Car\" WHERE \"Id\"=@id;";

                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();

                command.Parameters.AddWithValue("@id", id);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return BadRequest("Not found!");
                }
                connection.Close();
                return Ok();
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}

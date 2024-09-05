using IntroductionWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace IntroductionWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=postgres;Database=car-dealershop";

        [HttpGet]
        [Route("getCarTypeName")]
        public IActionResult Get()
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"SELECT \"Id\", \"Name\" FROM \"CarType\"";

                using var command = new NpgsqlCommand(commandText, connection);
                using var reader = command.ExecuteReader();

                var carTypes = new List<CarType>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var carType = new CarType();
                        carType.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                        carType.Name = reader.IsDBNull(1) ? null : reader[1].ToString();

                        carTypes.Add(carType);
                    }

                }
                connection.Close();
                return Ok(carTypes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetCarTypeName/{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var carType = new CarType();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                string commandText = $"SELECT \"Id\", \"Name\" FROM \"CarType\" WHERE \"Id\" = @id";
                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using NpgsqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    carType.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    carType.Name = reader.IsDBNull(1) ? null : reader[1].ToString();
                }
                if (carType == null)
                {
                    return NotFound();
                }
                connection.Close();
                return Ok(carType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("insertCarType")]
        public IActionResult InputCarType([FromBody] CarType carType)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "INSERT INTO \"CarType\" (\"Id\", \"Name\") VALUES (@id, @name)";

                connection.Open();

                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@name", carType.Name);

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
        [Route("updateCarTypeName/{id}")]
        public IActionResult UpdateNameById(Guid id, [FromBody] string name)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"UPDATE \"CarType\" SET \"Name\" = @name WHERE \"Id\" = @id";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return NotFound("CarType not found");
                }
                connection.Close();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("deleteCarType/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"DELETE FROM \"CarType\" WHERE \"Id\" = @id";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                int numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0)
                {
                    return NotFound("CarType not found");
                }
                connection.Close();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

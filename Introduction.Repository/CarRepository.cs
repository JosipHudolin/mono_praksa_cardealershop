using Introduction.Common;
using Introduction.Model;
using Introduction.Repository.Common;
using Npgsql;
using System.Text;

namespace Introduction.Repository
{
    public class CarRepository: ICarRepository
    {

        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=postgres;Database=car-dealershop";
        public async Task<Car> GetCarByIdAsync(Guid id)
        {
            try
            {
                var car = new Car();
                var carType = new CarType();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                string commandText = "SELECT c.\"Id\", c.\"Make\", c.\"Model\", c.\"Year\", c.\"Mileage\", c.\"Description\", c.\"InputDate\", \r\n       ct.\"Id\" as \"CarTypeId\", ct.\"Name\" as \"CarTypeName\"\r\nFROM \"Car\" c\r\nLEFT JOIN \"CarType\" ct ON c.\"CarTypeId\" = ct.\"Id\"\r\nWHERE c.\"Id\" = @id;\r\n";
                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    car.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    car.Make = reader.IsDBNull(1) ? null : reader[1].ToString();
                    car.Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString();
                    car.Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : reader.GetInt32(reader.GetOrdinal("Year"));
                    car.Mileage = reader.IsDBNull(reader.GetOrdinal("Mileage")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mileage"));
                    car.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));
                    car.InputDate = reader.IsDBNull(reader.GetOrdinal("InputDate")) ? (DateOnly?)null : reader.GetFieldValue<DateOnly?>(reader.GetOrdinal("InputDate"));
                    car.CarType = reader.IsDBNull(reader.GetOrdinal("CarTypeId")) ? null : new CarType
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CarTypeId")),
                        Name = reader.IsDBNull(reader.GetOrdinal("CarTypeName")) ? null : reader.GetString(reader.GetOrdinal("CarTypeName"))
                    };
                    car.CarTypeId = reader.IsDBNull(reader.GetOrdinal("CarTypeId")) ? Guid.Empty : Guid.Parse(reader["CarTypeId"].ToString());
                }
                else
                {
                    return null;
                }
                connection.Close();
                return car;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Car>> GetAllCarsAsync(AddFilter filter, Paging paging, Sorting sorting)
        {
            try
            {
                List<Car> cars = new List<Car>();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                var commandText = @"SELECT c.""Id"", c.""Make"", c.""Model"", c.""Year"", c.""Mileage"", c.""Description"", c.""InputDate"", 
                                   ct.""Id"" as ""CarTypeId"", ct.""Name"" as ""CarTypeName""
                            FROM ""Car"" c
                            LEFT JOIN ""CarType"" ct ON c.""CarTypeId"" = ct.""Id"" WHERE 1=1 ";
                
                var sb = new StringBuilder(commandText);
                if (!string.IsNullOrWhiteSpace(filter.Make))
                {
                    sb.Append($" AND c.\"Make\" = @filterMake");
                }
                if (!string.IsNullOrWhiteSpace(filter.Model))
                {
                    sb.Append($" AND c.\"Model\" = @filterModel");
                }
                if (filter.YearFrom != 0 && filter.YearTo != 0)
                {
                    sb.Append($" AND c.\"Year\" > @filterYearFrom AND c.\"Year\" < @filterYearTo");
                }
                if (filter.YearFrom != 0 && filter.YearTo == 0)
                {
                    sb.Append($" AND c.\"Year\" > @filterYearFrom");
                }
                if (filter.YearFrom == 0 && filter.YearTo != 0)
                {
                    sb.Append($" AND c.\"Year\" < @filterYearTo");
                }
                if (filter.MileageFrom != 0 && filter.MileageTo != 0)
                {
                    sb.Append($" AND c.\"Mileage\" > @filterMileageFrom AND c.\"Mileage\" < @filterMileageTo");
                }
                if (filter.MileageFrom != 0 && filter.MileageTo == 0)
                {
                    sb.Append($" AND c.\"Mileage\" > @filterMileageFrom");
                }
                if (filter.MileageFrom == 0 && filter.MileageTo != 0)
                {
                    sb.Append($" AND c.\"Mileage\" < @filterMileageTo");
                }
                if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
                {
                    sb.Append(" AND (\"Description\" ILIKE @searchQuery OR \"Make\" ILIKE @searchQuery OR \"Model\" ILIKE @searchQuery)");
                }
                if (filter.CarTypeId != null)
                {
                    sb.Append($" AND \"CarTypeId\" = @filterCarTypeId");
                }

                if (sorting != null)
                {
                    sb.Append($" ORDER BY \"{sorting.OrderBy}\" {sorting.SortDirection}");
                }

                if (paging != null)
                {
                    sb.Append($" LIMIT @rpp OFFSET @offset");
                }

                commandText = sb.ToString();

                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@filterMake", filter.Make);
                command.Parameters.AddWithValue("@filterModel", filter.Model);
                command.Parameters.AddWithValue("@filterYearFrom", filter.YearFrom);
                command.Parameters.AddWithValue("@filterYearTo", filter.YearTo);
                command.Parameters.AddWithValue("@filterMileageFrom", filter.MileageFrom);
                command.Parameters.AddWithValue("@filterMileageTo", filter.MileageTo);
                command.Parameters.AddWithValue("@searchQuery", $"%{filter.SearchQuery}%");
                command.Parameters.AddWithValue("@filterCarTypeId", filter.CarTypeId == null ? DBNull.Value : filter.CarTypeId);
                command.Parameters.AddWithValue("@rpp", paging.Rpp);
                command.Parameters.AddWithValue("offset", paging.Rpp * (paging.PageNumber - 1));

                connection.Open();
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var car = new Car
                        {
                            Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString()),
                            Make = reader.IsDBNull(1) ? null : reader[1].ToString(),
                            Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString(),
                            CarTypeId = reader.IsDBNull(reader.GetOrdinal("CarTypeId")) ? Guid.Empty : Guid.Parse(reader["CarTypeId"].ToString()),
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
                else
                {
                    return null;
                }

                connection.Close();

                return cars;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> InputCarAsync(Car car)
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
                command.Parameters.AddWithValue("@carTypeId", NpgsqlTypes.NpgsqlDbType.Uuid, car.CarTypeId);
                command.Parameters.AddWithValue("@year", car.Year);
                command.Parameters.AddWithValue("@mileage", car.Mileage);
                command.Parameters.AddWithValue("@description", car.Description);
                command.Parameters.AddWithValue("@inputDate", NpgsqlTypes.NpgsqlDbType.Date, DateOnly.FromDateTime(DateTime.Now));

                int numberOfCommits = await command.ExecuteNonQueryAsync();

                if (numberOfCommits == 0)
                {
                    return false;
                }

                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();
                var sb = new StringBuilder();
                using var command = new NpgsqlCommand(sb.ToString(), connection);
                sb.Append("UPDATE \"Car\" SET ");
                var currentCar = await GetByIdAsync((Guid)car.Id);
                if (currentCar == null) { return false; }
                if (!string.IsNullOrWhiteSpace(car.Description) && !string.Equals(car.Description, currentCar.Description))
                {
                    sb.Append("\"Description\" = @description,");
                    command.Parameters.AddWithValue("@description", car.Description);
                }
                if (car.Mileage != 0 && car.Mileage != currentCar.Mileage)
                {
                    sb.Append("\"Mileage\" = @mileage,");
                    command.Parameters.AddWithValue("@mileage", car.Mileage);
                }
                sb.Length -= 1;
                sb.Append(" WHERE \"Id\" = @id");
                command.CommandText = sb.ToString();
                command.Parameters.AddWithValue("@id", car.Id);

                int numberOfCommits = await command.ExecuteNonQueryAsync();

                if (numberOfCommits == 0)
                {
                    return false;
                }
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCarAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "DELETE FROM \"Car\" WHERE \"Id\"=@id;";

                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();

                command.Parameters.AddWithValue("@id", id);

                int numberOfCommits = await command.ExecuteNonQueryAsync();

                if (numberOfCommits == 0)
                {
                    return false;
                }
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Car> GetByIdAsync(Guid id)
        {
            try
            {
                var car = new Car();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                string commandText = "SELECT c.\"Id\", c.\"Make\", c.\"Model\", c.\"Year\", c.\"Mileage\", c.\"Description\", c.\"InputDate\", \r\n       ct.\"Id\" as \"CarTypeId\", ct.\"Name\" as \"CarTypeName\"\r\nFROM \"Car\" c\r\nLEFT JOIN \"CarType\" ct ON c.\"CarTypeId\" = ct.\"Id\"\r\nWHERE c.\"Id\" = @id;\r\n";
                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    reader.Read();

                    car.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    car.Make = reader.IsDBNull(1) ? null : reader[1].ToString();
                    car.Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString();
                    car.CarTypeId = reader.IsDBNull(3) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    car.Year = reader.IsDBNull(reader.GetOrdinal("Year")) ? 0 : reader.GetInt32(reader.GetOrdinal("Year"));
                    car.Mileage = reader.IsDBNull(reader.GetOrdinal("Mileage")) ? 0 : reader.GetInt32(reader.GetOrdinal("Mileage"));
                    car.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));
                    car.InputDate = reader.IsDBNull(reader.GetOrdinal("InputDate")) ? (DateOnly?)null : reader.GetFieldValue<DateOnly?>(reader.GetOrdinal("InputDate"));
                }
                connection.Close();
                return car;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

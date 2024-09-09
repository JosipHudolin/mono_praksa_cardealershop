using Introduction.Model;
using Introduction.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Repository
{
    public class CarTypeRepository: ICarTypeRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5432;Username=postgres;Password=postgres;Database=car-dealershop";
        public async Task<List<CarType>> Get()
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"SELECT \"Id\", \"Name\" FROM \"CarType\"";

                using var command = new NpgsqlCommand(commandText, connection);
                using var reader = await command.ExecuteReaderAsync();

                var carTypes = new List<CarType>();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var carType = new CarType();
                        carType.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                        carType.Name = reader.IsDBNull(1) ? null : reader[1].ToString();

                        carTypes.Add(carType);
                    }

                }
                connection.Close();
                return carTypes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CarType> GetById(Guid id)
        {
            try
            {
                var carType = new CarType();
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                string commandText = $"SELECT \"Id\", \"Name\" FROM \"CarType\" WHERE \"Id\" = @id";
                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();

                    carType.Id = reader.IsDBNull(0) ? Guid.Empty : Guid.Parse(reader[0].ToString());
                    carType.Name = reader.IsDBNull(1) ? null : reader[1].ToString();
                }
                if (carType == null)
                {
                    return null;
                }
                connection.Close();
                return carType;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> InputCarType(CarType carType)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);

                string commandText = "INSERT INTO \"CarType\" (\"Id\", \"Name\") VALUES (@id, @name)";

                connection.Open();

                using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@name", carType.Name);

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

        public async Task<bool> UpdateNameById(Guid id, string name)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"UPDATE \"CarType\" SET \"Name\" = @name WHERE \"Id\" = @id";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

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

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();

                string commandText = $"DELETE FROM \"CarType\" WHERE \"Id\" = @id";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

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
    }
}

using Microsoft.Data.SqlClient;
using System.Data;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Models;

namespace TravelAgency.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly string _connectionString;

        public DestinationRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET ALL DESTINATIONS
        public async Task<List<Destination>> GetAllAsync()
        {
            var destinations = new List<Destination>();

            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT DestinationId,
                       DestinationName,
                       Country,
                       Description,
                       CreatedAt
                FROM Destinations
                ORDER BY DestinationId DESC;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                destinations.Add(MapDestination(reader));
            }

            return destinations;
        }

        // GET DESTINATION BY ID
        public async Task<Destination?> GetByIdAsync(int id)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT DestinationId,
                       DestinationName,
                       Country,
                       Description,
                       CreatedAt
                FROM Destinations
                WHERE DestinationId = @Id;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapDestination(reader);
        }

        // CREATE DESTINATION
        public async Task<int> CreateAsync(Destination destination)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Destinations
                (
                    DestinationName,
                    Country,
                    Description
                )
                OUTPUT INSERTED.DestinationId
                VALUES
                (
                    @DestinationName,
                    @Country,
                    @Description
                );";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add(
                "@DestinationName", SqlDbType.NVarChar, 100)
                .Value = destination.DestinationName;

            command.Parameters.Add(
                "@Country", SqlDbType.NVarChar, 100)
                .Value = destination.Country;

            command.Parameters.Add(
                "@Description", SqlDbType.NVarChar, 500)
                .Value = (object?)destination.Description ?? DBNull.Value;

            await connection.OpenAsync();

            return Convert.ToInt32(
                await command.ExecuteScalarAsync());
        }

        // UPDATE DESTINATION
        public async Task<bool> UpdateAsync(
            int id, Destination destination)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                UPDATE Destinations
                SET DestinationName = @DestinationName,
                    Country = @Country,
                    Description = @Description
                WHERE DestinationId = @Id;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            command.Parameters.Add(
                "@DestinationName", SqlDbType.NVarChar, 100)
                .Value = destination.DestinationName;

            command.Parameters.Add(
                "@Country", SqlDbType.NVarChar, 100)
                .Value = destination.Country;

            command.Parameters.Add(
                "@Description", SqlDbType.NVarChar, 500)
                .Value = (object?)destination.Description ?? DBNull.Value;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // DELETE DESTINATION
        public async Task<bool> DeleteAsync(int id)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                DELETE FROM Destinations
                WHERE DestinationId = @Id;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            try
            {
                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException(
                    "Cannot delete destination because packages reference it.");
            }
        }

        // MAP SQL DATA TO ENTITY
        private static Destination MapDestination(SqlDataReader reader)
        {
            return new Destination
            {
                DestinationId = Convert.ToInt32(reader["DestinationId"]),
                DestinationName = reader["DestinationName"].ToString()!,
                Country = reader["Country"].ToString()!,
                Description = reader["Description"] == DBNull.Value
                    ? null
                    : reader["Description"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
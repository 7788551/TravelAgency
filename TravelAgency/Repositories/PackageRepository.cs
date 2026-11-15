using Microsoft.Data.SqlClient;
using System.Data;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Models;


namespace TravelAgency.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly string _connectionString;

        public PackageRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        // GET ALL PACKAGES
        public async Task<List<Package>> GetAllAsync()
        {
            var packages = new List<Package>();

            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT PackageId,
                       PackageName,
                       DestinationId,
                       Description,
                       Price,
                       DurationDays,
                       CreatedAt
                FROM Packages
                ORDER BY PackageId DESC;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                packages.Add(MapPackage(reader));
            }

            return packages;
        }

        // GET PACKAGE BY ID
        public async Task<Package?> GetByIdAsync(int id)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT PackageId,
                       PackageName,
                       DestinationId,
                       Description,
                       Price,
                       DurationDays,
                       CreatedAt
                FROM Packages
                WHERE PackageId = @Id;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapPackage(reader);
        }

        // CREATE PACKAGE
        public async Task<int> CreateAsync(Package package)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Packages
                (
                    PackageName,
                    DestinationId,
                    Description,
                    Price,
                    DurationDays
                )
                OUTPUT INSERTED.PackageId
                VALUES
                (
                    @PackageName,
                    @DestinationId,
                    @Description,
                    @Price,
                    @DurationDays
                );";

            using SqlCommand command =
                new SqlCommand(query, connection);

            AddPackageParameters(command, package);

            await connection.OpenAsync();

            try
            {
                return Convert.ToInt32(
                    await command.ExecuteScalarAsync());
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException(
                    "Selected destination does not exist.");
            }
        }

        // UPDATE PACKAGE
        public async Task<bool> UpdateAsync(
            int id, Package package)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                UPDATE Packages
                SET PackageName = @PackageName,
                    DestinationId = @DestinationId,
                    Description = @Description,
                    Price = @Price,
                    DurationDays = @DurationDays
                WHERE PackageId = @Id;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            AddPackageParameters(command, package);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            try
            {
                return await command.ExecuteNonQueryAsync() > 0;
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException(
                    "Selected destination does not exist.");
            }
        }

        // DELETE PACKAGE
        public async Task<bool> DeleteAsync(int id)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                DELETE FROM Packages
                WHERE PackageId = @Id;";

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
                    "Cannot delete package because customer enquiries reference it.");
            }
        }

        // COMMON PARAMETERS
        private static void AddPackageParameters(
            SqlCommand command, Package package)
        {
            command.Parameters.Add(
                "@PackageName", SqlDbType.NVarChar, 150)
                .Value = package.PackageName;

            command.Parameters.Add(
                "@DestinationId", SqlDbType.Int)
                .Value = package.DestinationId;

            command.Parameters.Add(
                "@Description", SqlDbType.NVarChar, 1000)
                .Value = (object?)package.Description ?? DBNull.Value;

            command.Parameters.Add(
                "@Price", SqlDbType.Decimal)
                .Value = package.Price;

            command.Parameters["@Price"].Precision = 12;
            command.Parameters["@Price"].Scale = 2;

            command.Parameters.Add(
                "@DurationDays", SqlDbType.Int)
                .Value = package.DurationDays;
        }

        // MAP SQL DATA TO ENTITY
        private static Package MapPackage(SqlDataReader reader)
        {
            return new Package
            {
                PackageId = Convert.ToInt32(reader["PackageId"]),
                PackageName = reader["PackageName"].ToString()!,
                DestinationId = Convert.ToInt32(reader["DestinationId"]),
                Description = reader["Description"] == DBNull.Value
                    ? null
                    : reader["Description"].ToString(),
                Price = Convert.ToDecimal(reader["Price"]),
                DurationDays = Convert.ToInt32(reader["DurationDays"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
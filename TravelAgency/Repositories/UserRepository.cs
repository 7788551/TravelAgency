using Microsoft.Data.SqlClient;
using System.Data;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Models;

namespace TravelAgency.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        // CUSTOMER REGISTRATION
        public async Task<int> RegisterCustomerAsync(User user)
        {
            return await InsertUserAsync(user, "Customer");
        }

        // CREATE STAFF
        public async Task<int> CreateStaffAsync(User user)
        {
            return await InsertUserAsync(user, "Staff");
        }

        // COMMON INSERT METHOD
        private async Task<int> InsertUserAsync(
            User user,
            string roleName)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Users
                (
                    FullName,
                    Email,
                    PasswordHash,
                    RoleId
                )
                OUTPUT INSERTED.UserId
                VALUES
                (
                    @FullName,
                    @Email,
                    @PasswordHash,
                    (SELECT RoleId FROM Roles
                     WHERE RoleName = @RoleName)
                );";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100)
                .Value = user.FullName;

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 150)
                .Value = user.Email;

            command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255)
                .Value = user.PasswordHash;

            command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 30)
                .Value = roleName;

            await connection.OpenAsync();

            object? result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                throw new InvalidOperationException("User role was not found.");

            return Convert.ToInt32(result);
        }

        // GET USER BY EMAIL
        public async Task<User?> GetByEmailAsync(string email)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    U.UserId,
                    U.FullName,
                    U.Email,
                    U.PasswordHash,
                    U.RoleId,
                    U.CreatedAt,
                    R.RoleName
                FROM Users U
                INNER JOIN Roles R
                    ON U.RoleId = R.RoleId
                WHERE U.Email = @Email;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 150)
                .Value = email;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                FullName = reader["FullName"].ToString()!,
                Email = reader["Email"].ToString()!,
                PasswordHash = reader["PasswordHash"].ToString()!,
                RoleId = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"].ToString()!,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }

        // GET ALL STAFF
        public async Task<List<User>> GetStaffAsync()
        {
            var staffList = new List<User>();

            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    U.UserId,
                    U.FullName,
                    U.Email,
                    U.RoleId,
                    U.CreatedAt,
                    R.RoleName
                FROM Users U
                INNER JOIN Roles R
                    ON U.RoleId = R.RoleId
                WHERE R.RoleName = 'Staff'
                ORDER BY U.UserId DESC;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                staffList.Add(new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    RoleName = reader["RoleName"].ToString()!,
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return staffList;
        }
    }
}
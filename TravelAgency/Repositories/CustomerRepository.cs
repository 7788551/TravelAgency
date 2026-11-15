using Microsoft.Data.SqlClient;
using System.Data;
using TravelAgency.Interfaces.Repositories;
using TravelAgency.Models;

namespace TravelAgency.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        // CREATE ENQUIRY
        public async Task<int> CreateEnquiryAsync(
            CustomerEnquiry enquiry)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO CustomerEnquiries
                (
                    UserId,
                    PackageId,
                    Message
                )
                OUTPUT INSERTED.EnquiryId
                VALUES
                (
                    @UserId,
                    @PackageId,
                    @Message
                );";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int)
                .Value = enquiry.UserId;

            command.Parameters.Add("@PackageId", SqlDbType.Int)
                .Value = enquiry.PackageId;

            command.Parameters.Add("@Message", SqlDbType.NVarChar, 1000)
                .Value = enquiry.Message;

            await connection.OpenAsync();

            try
            {
                return Convert.ToInt32(
                    await command.ExecuteScalarAsync());
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw new InvalidOperationException(
                    "Customer or selected package does not exist.");
            }
        }

        // GET CUSTOMER'S OWN ENQUIRIES
        public async Task<List<CustomerEnquiry>> GetMyEnquiriesAsync(
            int userId)
        {
            var enquiries = new List<CustomerEnquiry>();

            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    EnquiryId,
                    UserId,
                    PackageId,
                    Message,
                    Status,
                    CreatedAt
                FROM CustomerEnquiries
                WHERE UserId = @UserId
                ORDER BY EnquiryId DESC;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int)
                .Value = userId;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                enquiries.Add(MapEnquiry(reader));
            }

            return enquiries;
        }

        // GET ALL ENQUIRIES
        public async Task<List<CustomerEnquiry>> GetAllEnquiriesAsync()
        {
            var enquiries = new List<CustomerEnquiry>();

            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                SELECT
                    EnquiryId,
                    UserId,
                    PackageId,
                    Message,
                    Status,
                    CreatedAt
                FROM CustomerEnquiries
                ORDER BY EnquiryId DESC;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                enquiries.Add(MapEnquiry(reader));
            }

            return enquiries;
        }

        // UPDATE ENQUIRY STATUS
        public async Task<bool> UpdateEnquiryStatusAsync(
            int enquiryId, string status)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            string query = @"
                UPDATE CustomerEnquiries
                SET Status = @Status
                WHERE EnquiryId = @EnquiryId;";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.Add(
                "@Status", SqlDbType.NVarChar, 30)
                .Value = status;

            command.Parameters.Add(
                "@EnquiryId", SqlDbType.Int)
                .Value = enquiryId;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }

        // COMMON MAPPING METHOD
        private static CustomerEnquiry MapEnquiry(
            SqlDataReader reader)
        {
            return new CustomerEnquiry
            {
                EnquiryId = Convert.ToInt32(reader["EnquiryId"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                PackageId = Convert.ToInt32(reader["PackageId"]),
                Message = reader["Message"].ToString()!,
                Status = reader["Status"].ToString()!,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
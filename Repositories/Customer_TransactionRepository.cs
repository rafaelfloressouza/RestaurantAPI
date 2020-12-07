using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class Customer_TransactionRepository
    {
        private readonly string _connectionString;

        public Customer_TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Customer_Transaction records in the database
        public async Task<List<Customer_Transaction>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Transaction_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Customer_Transaction>();
                    await sql.OpenAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Function returns the Customer_Transaction with the specified user_id and transaction_id from the database
        public async Task<Customer_Transaction> GetById(int user_id, int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Transaction_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters[1].Value = tran_id;
                    Customer_Transaction response = null;
                    await sql.OpenAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }

                    return response;
                }
            }
        }

        // Function inserts a Customer_Transaction record in the database
        public async Task Insert(Customer_Transaction customer_transaction)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Transaction_InsertValue\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = customer_transaction.User_ID;
                    cmd.Parameters[1].Value = customer_transaction.Transaction_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Customer_Transaction record in the database
        public async Task DeleteById(int user_id, int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Transaction_DeleteById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters[1].Value = tran_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Customer_Transaction model
        private Customer_Transaction MapToValue(NpgsqlDataReader reader)
        {
            return new Customer_Transaction()
            {
                User_ID = (int)reader["User_ID"],
                Transaction_ID = (int)reader["Transaction_ID"],
            };
        }
    }
}

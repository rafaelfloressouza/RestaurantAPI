using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class Online_OrderRepository
    {
        private readonly string _connectionString;

        public Online_OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Online_Order records in the database
        public async Task<List<Online_Order>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOnline_Order_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Online_Order>();
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

        // Function returns the Online_Order with the specified order_id from the database
        public async Task<Online_Order> GetById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOnline_Order_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    Online_Order response = null;
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

        // Function inserts an Online_Order record in the database
        public async Task Insert(Online_Order online_order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOnline_Order_InsertValue\"", sql))     // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("application", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = online_order.Order_ID;
                    cmd.Parameters[1].Value = online_order.Application;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies an Online_Orders record in the database
        public async Task ModifyById(Online_Order online_order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOnline_Order_ModifyById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("application", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = online_order.Order_ID;
                    cmd.Parameters[1].Value = online_order.Application;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an Online_Order record in the database
        public async Task DeleteById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOnline_Order_DeleteById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Online_Order model
        private Online_Order MapToValue(NpgsqlDataReader reader)
        {
            return new Online_Order()
            {
                Order_ID = (int)reader["Order_ID"],
                Application = reader["Application"].ToString(),
            };
        }
    }
}

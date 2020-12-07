using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class In_Store_OrderRepository
    {
        private readonly string _connectionString;

        public In_Store_OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all In_Store_Order records in the database
        public async Task<List<In_Store_Order>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_GetAll\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<In_Store_Order>();
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

        // Function returns the In_Store_Order with the specified order_id from the database
        public async Task<In_Store_Order> GetById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_GetById\"", sql))  // Specifying stored procedure
                {   
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    In_Store_Order response = null;
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

        // Function inserts an In_Store_Order record in the database
        public async Task Insert(In_Store_Order in_store_order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_InsertValue\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = in_store_order.Order_ID;
                    cmd.Parameters[1].Value = in_store_order.TableNo;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies an In_Store_Order record in the database
        public async Task ModifyById(In_Store_Order in_store_order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_ModifyById\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = in_store_order.Order_ID;
                    cmd.Parameters[1].Value = in_store_order.TableNo;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an In_Store_Order record in the database
        public async Task DeleteById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_DeleteById\"", sql))   // Specifying stored procedure
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

        // Function returns the orders taken by the specified waiter
        public async Task<List<In_Store_Order>> getOrdersByWaiter(int waiter_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_GetOrdersByWaiter\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("waiter_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = waiter_id;
                    var response = new List<In_Store_Order>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

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

        // Function returns the orders that came from a specified table
        public async Task<List<In_Store_Order>> getOrdersByTable(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIn_Store_Order_GetOrdersByTable\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tableno;
                    var response = new List<In_Store_Order>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

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

        // Mapper used to map between the reader object and our In_Store_Order model
        private In_Store_Order MapToValue(NpgsqlDataReader reader)
        {
            return new In_Store_Order()
            {
                Order_ID = (int)reader["Order_ID"],
                TableNo = (int)reader["TableNo"],
            };
        }
    }
}

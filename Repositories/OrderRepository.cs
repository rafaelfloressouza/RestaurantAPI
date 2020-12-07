using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Order records in the database
        public async Task<List<Order>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Order>();
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

        // Function returns the Order with the specified order_id from the database
        public async Task<Order> GetById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    Order response = null;
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

        // Function inserts an Order record in the database
        public async Task Insert(Order order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("date_time", NpgsqlDbType.Timestamp));
                    cmd.Parameters[0].Value = order.User_ID;
                    cmd.Parameters[1].Value = order.Transaction_ID;
                    cmd.Parameters[2].Value = order.Date_Time;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies an Order record in the database
        public async Task ModifyById(Order order)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_ModifyById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("date_time", NpgsqlDbType.Timestamp));
                    cmd.Parameters[0].Value = order.Order_ID;
                    cmd.Parameters[1].Value = order.User_ID;
                    cmd.Parameters[2].Value = order.Transaction_ID;
                    cmd.Parameters[3].Value = order.Date_Time;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an Order record in the database
        public async Task DeleteById(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_DeleteById\"", sql))    // Specifying stored procedure
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

        // Function returns the order_id of the last inserted order
        public async Task<int> getLastOrderInserted()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_GetLastInserted\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("cur_order_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[0].Value);
                }
            }
        }

        // Function returns the number of orders in a transcation
        public async Task<int> numOrderByTransaction(int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_NumOrdersByTransaction\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tran_id;
                    cmd.Parameters.Add(new NpgsqlParameter("num_orders", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[1].Value);
                }
            }
        }

        // Function returns the total cost of an order (in dollars)
        public async Task<decimal> getCost(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {   
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_GetCost\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters.Add(new NpgsqlParameter("order_cost", NpgsqlDbType.Numeric) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToDecimal(cmd.Parameters[1].Value);
                }
            }
        }

        // Function returns all dishes included in an order
        public async Task<List<Dish>> getDishes(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_GetDishes\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = order_id;
                    var response = new List<Dish>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDish(reader));
                        }
                    }
                    return response;
                }
            }
        }

        // Mapper used to map between the reader object and our Order model
        private Order MapToValue(NpgsqlDataReader reader)
        {
            return new Order()
            {
                Order_ID = (int)reader["Order_ID"],
                User_ID = (int)reader["User_ID"],
                Transaction_ID = (int)reader["Transaction_ID"],
                Date_Time = (DateTime)reader["Date_Time"]
            };
        }

        // Mapper used to map between the reader object and our Dish model
        private Dish MapToDish(NpgsqlDataReader reader)
        {
            return new Dish()
            {
                Dish_ID = (int)reader["Dish_ID"],
                Available = (bool)reader["Available"],
                Price = (Decimal)reader["Price"],
                Description = (string)reader["Description"],
                Menu_Type = (string)reader["Menu_Type"]
            };
        }
    }
}

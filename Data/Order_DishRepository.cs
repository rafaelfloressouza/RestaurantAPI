using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;
using System;

namespace RestaurantAPI.Data
{
    public class Order_DishRepository
    {
        private readonly string _connectionString;

        public Order_DishRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public async Task<List<Order_Dish>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Dish_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Order_Dish>();
                    await sql.OpenAsync();

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

        private Order_Dish MapToValue(NpgsqlDataReader reader)
        {
            return new Order_Dish()
            {
                Dish_ID = (int)reader["Dish_ID"],
                Order_ID = (int)reader["Order_ID"],
            };
        }

        public async Task<Order_Dish> GetById(int order_id, int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Dish_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters[1].Value = dish_id;
                    Order_Dish response = null;
                    await sql.OpenAsync();

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

        public async Task Insert(Order_Dish order_dish)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Dish_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_dish.Order_ID;
                    cmd.Parameters[1].Value = order_dish.Dish_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task DeleteById(int order_id, int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Dish_DeleteById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters[1].Value = dish_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task<int> getNumberOfDishes(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Dish_getNumberOfDishes\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters.Add(new NpgsqlParameter("num_dishes", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[1].Value);
                }
            }
        }
    }
}

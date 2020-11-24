using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class DishRepository
    {
        private readonly string _connectionString;

        public DishRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public async Task<List<Dish>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Dish>();
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

        private Dish MapToValue(NpgsqlDataReader reader)
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

        public async Task<Dish> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;

                    Dish response = null;
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

        public async Task Insert(Dish dish)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("available", NpgsqlDbType.Boolean));
                    cmd.Parameters.Add(new NpgsqlParameter("price", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("menu_type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish.Dish_ID;
                    cmd.Parameters[1].Value = dish.Available;
                    cmd.Parameters[2].Value = dish.Price;
                    cmd.Parameters[3].Value = dish.Description;
                    cmd.Parameters[4].Value = dish.Menu_Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task ModifyById(Dish dish)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_ModifyById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("available", NpgsqlDbType.Boolean));
                    cmd.Parameters.Add(new NpgsqlParameter("price", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("menu_type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish.Dish_ID;
                    cmd.Parameters[1].Value = dish.Available;
                    cmd.Parameters[2].Value = dish.Price;
                    cmd.Parameters[3].Value = dish.Description;
                    cmd.Parameters[4].Value = dish.Menu_Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_DeleteById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}


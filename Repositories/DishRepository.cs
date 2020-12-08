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

        // Function returns all Dish records in the database
        public async Task<List<Dish>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Dish>();
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

        // Function returns the Dish with the specified dish_id from the database
        public async Task<Dish> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Dish response = null;
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

        // Function inserts a Dish record in the database
        public async Task Insert(Dish dish)
        {   
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_InsertValue\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("available", NpgsqlDbType.Boolean));
                    cmd.Parameters.Add(new NpgsqlParameter("price", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("menu_type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish.Available;
                    cmd.Parameters[1].Value = dish.Price;
                    cmd.Parameters[2].Value = dish.Description;
                    cmd.Parameters[3].Value = dish.Menu_Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies a Dish record in the database
        public async Task ModifyById(Dish dish)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_ModifyById\"", sql)) // Specifying stored procedure
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

        // Function deletes a Dish record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {   
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_DeleteById\"", sql)) // Specifying stored procedure
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

        // Function returns the id of the last inserted dish
        public async Task<int> getLastInserted()
        {   
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_LastInserted\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("cur_dish_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[0].Value);
                }
            }
        }

        // Function returns the ingredients in a specified dish
        public async Task<List<string>> getIngredients(int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetIngredients\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = dish_id;
                    var response = new List<string>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(reader["Name"].ToString());
                        }
                    }
                    return response;
                }
            }
        }

        // Functions returns all the available dishes
        public async Task<List<Dish>> getAvailable()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_GetAvailable\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Dish>();
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

        // Function changes the status of a dish to available
        public async Task makeAvailable(int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_MakeAvailable\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = dish_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function changes the status of a dish to unavailable
        public async Task makeUnavailable(int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_MakeUnavailable\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = dish_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Dish model
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
    }
}


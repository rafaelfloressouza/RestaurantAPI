using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class Dish_IngredientRepository
    {
        private readonly string _connectionString;

        public Dish_IngredientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Dish_Ingredient records in the database
        public async Task<List<Dish_Ingredient>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_Ingredient_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Dish_Ingredient>();
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

        // Function returns the Dish_Ingredient with the specified dish_id and ing_name from the database
        public async Task<Dish_Ingredient> GetById(int dish_id, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_Ingredient_GetById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish_id;
                    cmd.Parameters[1].Value = ing_name;
                    Dish_Ingredient response = null;
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

        // Function inserts a Dish_Ingredient record in the database
        public async Task Insert(Dish_Ingredient dish_ingredient)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_Ingredient_InsertValue\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish_ingredient.Dish_ID;
                    cmd.Parameters[1].Value = dish_ingredient.Ing_Name;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Dish_Ingredient record in the database
        public async Task DeleteById(int dish_id, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spDish_Ingredient_DeleteById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = dish_id;
                    cmd.Parameters[1].Value = ing_name;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns the number of ingredients contained in a dish
        public async Task<int> numIngredientsInDish(int dish_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Ingredient_NumIngredientInDish\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("dish_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = dish_id;
                    cmd.Parameters.Add(new NpgsqlParameter("num_ing", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[1].Value);
                }
            }
        }

        // Function returns the ingredients supplied by a specific supplier
        public async Task<List<string>> getIngredientsBySupplier(string supplier)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_GetIngredientsBySupplier\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("supplier", NpgsqlDbType.Varchar) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = supplier;
                    var response = new List<string>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(reader["Ing_Name"].ToString());
                        }
                    }
                    return response;
                }
            }
        }

        // Mapper used to map between the reader object and our Dish_Ingredient model
        private Dish_Ingredient MapToValue(NpgsqlDataReader reader)
        {
            return new Dish_Ingredient()
            {
                Dish_ID = (int)reader["Dish_ID"],
                Ing_Name = reader["Ing_Name"].ToString(),
            };
        }
    }
}

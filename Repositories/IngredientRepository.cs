using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class IngredientRepository
    {
        private readonly string _connectionString;

        public IngredientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Ingredient records in the database
        public async Task<List<Ingredient>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_GetAll\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Ingredient>();
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

        // Function returns the Ingredient with the specified ing_name from the database
        public async Task<Ingredient> GetByName(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_GetByName\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = ing_name;
                    Ingredient response = null;
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

        // Function inserts an Ingredient record in the database
        public async Task Insert(Ingredient ingredient)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_InsertValue\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("price", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("exp_date",NpgsqlDbType.Date));
                    cmd.Parameters.Add(new NpgsqlParameter("quantity", NpgsqlDbType.Numeric));
                    cmd.Parameters[0].Value = ingredient.Name;
                    cmd.Parameters[1].Value = ingredient.Price;
                    cmd.Parameters[2].Value = ingredient.Exp_Date;
                    cmd.Parameters[3].Value = ingredient.Quantity;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies an Ingredient record in the database
        public async Task ModifyByName(Ingredient ingredient)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_ModifyByName\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("price", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("exp_date", NpgsqlDbType.Date));
                    cmd.Parameters.Add(new NpgsqlParameter("quantity", NpgsqlDbType.Numeric));
                    cmd.Parameters[0].Value = ingredient.Name;
                    cmd.Parameters[1].Value = ingredient.Price;
                    cmd.Parameters[2].Value = ingredient.Exp_Date;
                    cmd.Parameters[3].Value = ingredient.Quantity;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an Ingredient record in the database
        public async Task DeleteByName(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_DeleteByName\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = ing_name;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Ingredients model
        private Ingredient MapToValue(NpgsqlDataReader reader)
        {
            return new Ingredient()
            {
                Name = reader["Name"].ToString(),
                Price = (decimal)reader["Price"],
                Exp_Date = (DateTime)reader["Exp_Date"],
                Quantity = (decimal)reader["Quantity"]
            };
        }
    }
}

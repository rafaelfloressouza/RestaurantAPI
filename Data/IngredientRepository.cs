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

        public async Task<List<Ingredient>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Ingredient>();
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

        private Ingredient MapToValue(NpgsqlDataReader reader)
        {
            return new Ingredient()
            {
                Name = reader["Name"].ToString(),
                Price =  (decimal)reader["Price"],
                Exp_Date = (DateTime)reader["Exp_Date"],
                Quantity = (decimal)reader["Quantity"]
            };
        }

        public async Task<Ingredient> GetByName(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_GetByName\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = ing_name;
                    Ingredient response = null;
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

        public async Task Insert(Ingredient ingredient)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_InsertValue\"", sql))
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

        public async Task ModifyByName(Ingredient ingredient)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_ModifyByName\"", sql))
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

        public async Task DeleteByName(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_DeleteByName\"", sql))
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
    }
}

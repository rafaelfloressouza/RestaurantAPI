using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;
using System;

namespace RestaurantAPI.Data
{
    public class Ingredient_SupplierRepository 
    {
        private readonly string _connectionString;

        public Ingredient_SupplierRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Ingredient_Supplier records in the database
        public async Task<List<Ingredient_Supplier>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Ingredient_Supplier>();
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

        // Function returns the Ingredient_Supplier record with the specified supplier and ing_name from the database
        public async Task<Ingredient_Supplier> GetById(string supplier, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_GetById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("supplier", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = supplier;
                    cmd.Parameters[1].Value = ing_name;
                    Ingredient_Supplier response = null;
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

        // Function inserts an Ingredient_Supplier record in the database
        public async Task Insert(Ingredient_Supplier ing_sup)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_InsertValue\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("supplier", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = ing_sup.Supplier;
                    cmd.Parameters[1].Value = ing_sup.Ing_Name;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an Ingredient_Supplier record in the database
        public async Task DeleteById(string supplier, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_DeleteById\"", sql))  // Specifying stored procedure
                {   
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("supplier", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = supplier;
                    cmd.Parameters[1].Value = ing_name;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns the numer of suppliers of a given ingredient
        public async Task<int> getNumberOfSuppliers(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_getNumberOfSuppliers\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar) { Direction = System.Data.ParameterDirection.Input});
                    cmd.Parameters[0].Value = ing_name;
                    cmd.Parameters.Add(new NpgsqlParameter("num_suppliers", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[1].Value);
                }
            }
        }

        // Mapper used to map between the reader object and our Ingredient_Supplier model
        private Ingredient_Supplier MapToValue(NpgsqlDataReader reader)
        {
            if (Convert.IsDBNull(reader["Supplier"]) || Convert.IsDBNull(reader["Ing_Name"]))
            {
                throw new ArgumentNullException();
            }

            return new Ingredient_Supplier()
            {
                Supplier = reader["Supplier"].ToString(),
                Ing_Name = reader["Ing_Name"].ToString(),
            };
        }

    }
}

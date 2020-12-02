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

        public async Task<List<Ingredient_Supplier>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Ingredient_Supplier>();
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

        private Ingredient_Supplier MapToValue(NpgsqlDataReader reader)
        {
            if(Convert.IsDBNull(reader["Supplier"]) || Convert.IsDBNull(reader["Ing_Name"]))
            {
                throw new ArgumentNullException();
            }

            return new Ingredient_Supplier()
            {
                Supplier = reader["Supplier"].ToString(),
                Ing_Name = reader["Ing_Name"].ToString(),
            };
        }

        public async Task<Ingredient_Supplier> GetById(string supplier, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("supplier", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("ing_name", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = supplier;
                    cmd.Parameters[1].Value = ing_name;
                    Ingredient_Supplier response = null;
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

        public async Task Insert(Ingredient_Supplier ing_sup)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_InsertValue\"", sql))
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

        public async Task DeleteById(string supplier, string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_DeleteById\"", sql))
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

        public async Task<int> getNumberOfSuppliers(string ing_name)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spIngredient_Supplier_getNumberOfSuppliers\"", sql))
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
    }
}

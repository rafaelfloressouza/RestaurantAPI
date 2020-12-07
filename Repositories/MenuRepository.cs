using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class MenuRepository
    {
        private readonly string _connectionString;

        public MenuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Menu records in the database
        public async Task<List<Menu>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Menu>();
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

        // Function returns the Menu with the specified type from the database
        public async Task<Menu> GetByType(string type)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_GetByType\"", sql))   // Specifying stored procedure
                {   
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = type;
                    Menu response = null;
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

        // Function inserts a Menu record in the database
        public async Task Insert(Menu menu)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_InsertValue\"", sql))     // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("available", NpgsqlDbType.Boolean));
                    cmd.Parameters[0].Value = menu.Type;
                    cmd.Parameters[1].Value = menu.Available;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies a Menu record in the database
        public async Task ModifyByType(Menu menu)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_ModifyByType\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("available", NpgsqlDbType.Boolean));
                    cmd.Parameters[0].Value = menu.Type;
                    cmd.Parameters[1].Value = menu.Available;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Menu record in the database
        public async Task DeleteByType(string type)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_DeleteByType\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function gets all dishes included in a menu
        public async Task<List<Dish>> getDishes(string type)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_GetDishes\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = type;
                    var response = new List<Dish>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

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

        // Function gets all the available menus
        public async Task<List<Menu>> getAvailable()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spMenu_GetAvailable\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Menu>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

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

        // Mapper used to map between the reader object and our Menus model
        private Menu MapToValue(NpgsqlDataReader reader)
        {
            return new Menu()
            {
                Type = reader["Type"].ToString(),
                Available = (bool)reader["Available"],
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

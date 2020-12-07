using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class CookRepository
    {
        private readonly string _connectionString;

        public CookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Cook records in the database
        public async Task<List<Cook>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString)) // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCook_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Cook>();
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

        // Function returns the Cook with the specified id from the database
        public async Task<Cook> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCook_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Cook response = null;
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

        // Function inserts a cook record in the database
        public async Task Insert(Cook cook)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCook_InsertValue\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("specialty", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = cook.User_ID;
                    cmd.Parameters[1].Value = cook.Specialty;
                    cmd.Parameters[2].Value = cook.Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies a cook record in the database
        public async Task ModifyById(Cook cook)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCook_ModifyById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("specialty", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = cook.User_ID;
                    cmd.Parameters[1].Value = cook.Specialty;
                    cmd.Parameters[2].Value = cook.Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a cook record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCook_DeleteById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Cook model
        private Cook MapToValue(NpgsqlDataReader reader)
        {
            return new Cook()
            {
                User_ID = (int)reader["User_ID"],
                Specialty = reader["Specialty"].ToString(),
                Type = reader["Type"].ToString()

            };
        }
    }
}

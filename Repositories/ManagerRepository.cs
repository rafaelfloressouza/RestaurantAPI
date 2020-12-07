using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System;

namespace RestaurantAPI.Data
{
    public class ManagerRepository
    {
        private readonly string _connectionString;

        public ManagerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");    
        }

        // Function returns all Manager records in the database
        public async Task<List<Manager>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<Manager>();
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

        // Function returns the Manager with the specified id from the database
        public async Task<Manager> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_GetById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Manager response = null;
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

        // Function inserts a Manager record in the database
        public async Task Insert(Manager manager)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_InsertValue\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("area", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = manager.User_ID;
                    cmd.Parameters[1].Value = manager.Area;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }


        // Function modifies a Managers record in the database
        public async Task ModifyById(Manager manager)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_ModifyById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("area", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = manager.User_ID;
                    cmd.Parameters[1].Value = manager.Area;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Manager record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_DeleteById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Functionn returns the number of manager in the database
        public async Task<int> getManagerNum()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spManager_GetNumberManagers\"", sql))   // Specifying stored procedure
                {   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("num_of_managers", NpgsqlDbType.Integer) { Direction = ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[0].Value);
                }
            }
        }

        // Mapper used to map between the reader object and our Manager model
        private Manager MapToValue(NpgsqlDataReader reader)
        {
            return new Manager()
            {
                User_ID = (int)reader["User_ID"],
                Area = reader["Area"].ToString(),
            };
        }
    }
}

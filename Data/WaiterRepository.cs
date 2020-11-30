using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class WaiterRepository
    {
        private readonly string _connectionString;

        public WaiterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public async Task<List<Waiter>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spWaiter_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Waiter>();
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

        public async Task<Waiter> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spWaiter_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Waiter response = null;
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

        public async Task Insert(Waiter waiter)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spWaiter_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("hours", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = waiter.User_ID;
                    cmd.Parameters[1].Value = waiter.Hours;
                    cmd.Parameters[2].Value = waiter.Type;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task ModifyById(Waiter waiter)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spWaiter_ModifyById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("hours", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlDbType.Varchar));
                    cmd.Parameters[0].Value = waiter.User_ID;
                    cmd.Parameters[1].Value = waiter.Hours;
                    cmd.Parameters[2].Value = waiter.Type;
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
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spWaiter_DeleteById\"", sql))
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

        private Waiter MapToValue(NpgsqlDataReader reader)
        {
            return new Waiter()
            {
                User_ID = (int)reader["User_ID"],
                Hours = (decimal)reader["Hours"],
                Type = reader["Type"].ToString()
            };
        }
    }
}

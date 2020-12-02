using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class TableRepository
    {
        private readonly string _connectionString;

        public TableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public async Task<List<Table>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Table>();
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

        private Table MapToValue(NpgsqlDataReader reader)
        {
            return new Table()
            {
                TableNo = (int)reader["TableNo"],
                Location = reader["Location"].ToString(),
                isOccupied = (bool)reader["isOccupied"],
                waiter_ID = (int)reader["waiter_ID"]
                
            };
        }

        public async Task<Table> GetById(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = tableno;
                    Table response = null;
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

        public async Task Insert(Table table)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("location",NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("isoccupied", NpgsqlDbType.Boolean));
                    cmd.Parameters.Add(new NpgsqlParameter("waiter_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = table.Location;
                    cmd.Parameters[1].Value = table.isOccupied;
                    cmd.Parameters[2].Value = table.waiter_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task ModifyById(Table table)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_ModifyById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("location", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("isoccupied", NpgsqlDbType.Boolean));
                    cmd.Parameters.Add(new NpgsqlParameter("waiter_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = table.TableNo;
                    cmd.Parameters[1].Value = table.Location;
                    cmd.Parameters[2].Value = table.isOccupied;
                    cmd.Parameters[3].Value = table.waiter_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task DeleteById(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_DeleteById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = tableno;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}

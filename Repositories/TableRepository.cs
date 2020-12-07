using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
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

        // Function returns all Table records in the database
        public async Task<List<Table>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetAll\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Table>();
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

        // Function returns the Table with the specified tableno from the database
        public async Task<Table> GetById(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetById\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = tableno;
                    Table response = null;
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

        // Function inserts a Table record in the database
        public async Task Insert(Table table)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_InsertValue\"", sql))   // Specifying stored procedure
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

        // Function modifies a Table record in the database
        public async Task ModifyById(Table table)
        {   
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_ModifyById\"", sql))    // Specifying stored procedure
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

        // Function deletes a Table record in the database
        public async Task DeleteById(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_DeleteById\"", sql))    // Specifying stored procedure
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

        // Function returns Tables waited by a waiter/waitress
        public async Task<List<Table>> getWaitedBy(int waiter_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetWaitedBy\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("waiter_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = waiter_id;
                    var response = new List<Table>();
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

        // Function make the availability of a tale false
        public async Task makeOcuppied(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_MakeOccupied\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tableno;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function makes the availability of a table trues
        public async Task makeDisoccupied(int? tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_MakeDisoccupied\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tableno;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns all occupied tables
        public async Task<List<Table>> getOccupied()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTable_GetOccupied\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Table>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

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

        // Mapper used to map between the reader object and our Table model
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
    }
}

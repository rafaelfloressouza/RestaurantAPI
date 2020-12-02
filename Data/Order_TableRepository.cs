using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using NpgsqlTypes;
using System;

namespace RestaurantAPI.Data
{
    public class Order_TableRepository
    {
        private readonly string _connectionString;

        public Order_TableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        public async Task<List<Order_Table>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Table_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Order_Table>();
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

        private Order_Table MapToValue(NpgsqlDataReader reader)
        {
            return new Order_Table()
            {
                Order_ID = (int)reader["Order_ID"],
                TableNo = (int)reader["Table_No"],
            };
        }

        public async Task<Order_Table> GetById(int order_id, int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Table_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tabbleno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters[1].Value = tableno;
                    Order_Table response = null;
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

        public async Task Insert(Order_Table order_table)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Table_InsertValue\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_table.Order_ID;
                    cmd.Parameters[1].Value = order_table.TableNo;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task DeleteById(int order_id, int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Table_DeleteById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("table_no", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters[1].Value = tableno;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task<bool> orderExists(int order_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spOrder_Table_OrderExsits\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("order_id", NpgsqlDbType.Varchar) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = order_id;
                    cmd.Parameters.Add(new NpgsqlParameter("exsits", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToBoolean(cmd.Parameters[1].Value);
                }
            }
        }
    }
}

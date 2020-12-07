using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class TransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Transaction records in the database
        public async Task<List<Transaction>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_GetAll\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Transaction>();
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

        // Function returns the Transaction with the specified tran_id from the database
        public async Task<Transaction> GetById(int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_GetById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = tran_id;
                    Transaction response = null;
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

        // Function inserts a Transaction record in the database
        public async Task Insert(Transaction transaction)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_InsertValue\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("amount", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("date_time", NpgsqlDbType.Timestamp));
                    cmd.Parameters[0].Value = transaction.Amount;
                    cmd.Parameters[1].Value = transaction.Date_Time;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies a Transaction record in the database
        public async Task ModifyById(Transaction transaction)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_ModifyById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("amount", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("date_time", NpgsqlDbType.Timestamp));
                    cmd.Parameters[0].Value = transaction.Transaction_ID;
                    cmd.Parameters[1].Value = transaction.Amount;
                    cmd.Parameters[2].Value = transaction.Date_Time;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Transaction record in the database
        public async Task DeleteById(int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_DeleteById\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = tran_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns the tran_id of the last inserted transaction 
        public async Task<int> getLastTransactionInserted()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_GetLastInserted\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("cur_tran_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[0].Value);
                }
            }
        }

        // Function returns the amount (in dollars) of a transaction
        public async Task<decimal> getAmount(int tran_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_GetAmount\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tran_id;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_amount", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToDecimal(cmd.Parameters[1].Value);
                }
            }
        }

        // Function updated the amount (in dollars) of a transaction
        public async Task updateAmount(int tran_id, decimal new_amount)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spTransaction_UpdateAmount\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tran_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters.Add(new NpgsqlParameter("new_amount", NpgsqlDbType.Numeric) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tran_id;
                    cmd.Parameters[1].Value = new_amount;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Mapper used to map between the reader object and our Transaction model
        private Transaction MapToValue(NpgsqlDataReader reader)
        {
            return new Transaction()
            {
                Transaction_ID = (int)reader["Transaction_ID"],
                Amount = (decimal)reader["Amount"],
                Date_Time = (DateTime)reader["Date_Time"]
            };
        }
    }
}

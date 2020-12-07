using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Customer records in the database
        public async Task<List<Customer>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Customer>();
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

        // Function returns the Customer with the specified user_id from the database
        public async Task<Customer> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter<int>("id", NpgsqlDbType.Integer) { TypedValue = id });
                    Customer response = null;
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

        // Function inserts a Customer record in the database
        public async Task Insert(Customer customer)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_InsertValue\"", sql))    // Specifying stored procedure
                {   
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("table_no", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = customer.User_ID;
                    if (customer.TableNo == null) cmd.Parameters[1].Value = DBNull.Value;
                    else cmd.Parameters[1].Value = customer.TableNo;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function "Sits" a customer at a table
        public async Task Sit(int user_id, int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Sit\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    cmd.Parameters[1].Value = tableno;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function makes a customer "Leave" the table in which he/she is currently sitting
        public async Task Leave(int user_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_Leave\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a Customer record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_DeleteById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns all customers sitting at a specified table
        public async Task<List<Customer>> atTable(int tableno)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_AtTable\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("tableno", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = tableno;
                    var response = new List<Customer>();
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

        // Function returns all transaction associated with a customer
        public async Task<List<Transaction>> getTransactions(int user_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_GetTransactions\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    var response = new List<Transaction>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTransaction(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Function returns all reviews written by a customer
        public async Task<List<Review>> getReviews(int user_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_GetReviews\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    var response = new List<Review>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToReview(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Function returns all orders related to a customer
        public async Task<List<Order>> getOrders(int user_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spCustomer_GetOrders\"", sql))  // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = user_id;
                    var response = new List<Order>();
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Parsing the data retrieved from the database
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToOrder(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Mapper used to map between the reader object and our Customer model
        private Customer MapToValue(NpgsqlDataReader reader)
        {
            int? t = null;
            if (!Convert.IsDBNull(reader["TableNo"]))
            {
                t = (int?)reader["TableNo"];
            }

            return new Customer()
            {
                User_ID = (int)reader["User_ID"],
                TableNo = t,
            };
        }

        // Mapper used to map between the reader object and our Transaction model
        private Transaction MapToTransaction(NpgsqlDataReader reader)
        {
            return new Transaction()
            {
                Transaction_ID = (int)reader["Transaction_ID"],
                Amount = (decimal)reader["Amount"],
                Date_Time = (DateTime)reader["Date_Time"]
            };
        }

        // Mapper used to map between the reader object and our Review model
        private Review MapToReview(NpgsqlDataReader reader)
        {
            int? t = null;
            if (!Convert.IsDBNull(reader["Dish_ID"]))
            {
                t = (int?)reader["Dish_ID"];
            }

            return new Review()
            {
                User_ID = (int)reader["User_ID"],
                Review_ID = (int)reader["Review_ID"],
                Description = reader["Description"].ToString(),
                Rating = (int)reader["Rating"],
                Dish_ID = t
            };
        }

        // Mapper used to map between the reader object and our Order model
        private Order MapToOrder(NpgsqlDataReader reader)
        {
            return new Order()
            {
                Order_ID = (int)reader["Order_ID"],
                User_ID = (int)reader["User_ID"],
                Transaction_ID = (int)reader["Transaction_ID"],
                Date_Time = (DateTime)reader["Date_Time"]
            };
        }
    }
}

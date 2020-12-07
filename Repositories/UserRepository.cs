using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System.Data;

namespace RestaurantAPI.Data
{
    public class UserRepository {

        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all User records in the database
        public async Task<List<User>> GetAll()
        {
            using(NpgsqlConnection sql = new NpgsqlConnection(_connectionString))   // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var response = new List<User>();
                    await sql.OpenAsync();

                    // Parsing the data retrieved from the database
                    using ( var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;
                }
            }
        }

        // Function returns the User with the specified id from the database
        public async Task<User> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter<int> ("id", NpgsqlTypes.NpgsqlDbType.Integer) { TypedValue = id});
                    User response = null;
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

        // Function inserts a User record in the database
        public async Task Insert(User user)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_InsertValue\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter<string>("username", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Username});
                    cmd.Parameters.Add(new NpgsqlParameter<string>("password", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Password });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("firstname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.FirstName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("middlename", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.MiddleName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("lastname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.LastName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("givenname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.GivenName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("addr1", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Addr1 });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("addr2", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Addr2 });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("province", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Province });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("postalcode", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.PostalCode });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("sex", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Sex });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("phone", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Phone });
                    cmd.Parameters.Add(new NpgsqlParameter<DateTime>("dob", NpgsqlTypes.NpgsqlDbType.Timestamp) { TypedValue = user.DOB });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("email", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Email });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Function modifies a User record in the database
        public async Task ModifyById(User user)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_ModifyById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter<int>("id", NpgsqlTypes.NpgsqlDbType.Integer) { TypedValue = user.ID });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("username", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Username });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("password", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Password });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("firstname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.FirstName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("middlename", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.MiddleName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("lastname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.LastName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("givenname", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.GivenName });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("addr1", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Addr1 });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("addr2", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Addr2 });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("province", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Province });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("postalcode", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.PostalCode });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("sex", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Sex });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("phone", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Phone });
                    cmd.Parameters.Add(new NpgsqlParameter<DateTime>("dob", NpgsqlTypes.NpgsqlDbType.Timestamp) { TypedValue = user.DOB });
                    cmd.Parameters.Add(new NpgsqlParameter<string>("email", NpgsqlTypes.NpgsqlDbType.Varchar) { TypedValue = user.Email });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes a User record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_DeleteById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter<int>("id", NpgsqlTypes.NpgsqlDbType.Integer) { TypedValue = id});
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function returns the user_id of the last inserted user
        public async Task<int> getLastInsertedID()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spUser_LastInserted\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("cur_user_id", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Output });
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.Parameters[0].Value);
                    
                }
            }
        }

        // Mapper used to map between the reader object and our User model
        private User MapToValue(NpgsqlDataReader reader)
        {
            return new User()
            {
                ID = (int)reader["ID"],
                Username = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                MiddleName = reader["MiddleName"].ToString(),
                LastName = reader["LastName"].ToString(),
                GivenName = reader["GivenName"].ToString(),
                Addr1 = reader["Addr1"].ToString(),
                Addr2 = reader["Addr2"].ToString(),
                Province = reader["Province"].ToString(),
                PostalCode = reader["PostalCode"].ToString(),
                Sex = reader["Sex"].ToString(),
                Phone = reader["Phone"].ToString(),
                DOB = (DateTime)reader["DOB"],
                Email = reader["Email"].ToString(),
            };
        }
    }
}

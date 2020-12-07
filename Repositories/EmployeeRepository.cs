using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Models;
using Npgsql;
using System;
using NpgsqlTypes;

namespace RestaurantAPI.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connection");
        }

        // Function returns all Employee records in the database
        public async Task<List<Employee>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_GetAll\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Employee>();
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

        // Function returns the Employee with the specified user_id from the database
        public async Task<Employee> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_GetById\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Employee response = null;
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

        // Function inserts an Employee record in the database
        public async Task Insert(Employee employee)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_InsertValue\"", sql))    // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("start_date", NpgsqlDbType.Timestamp));
                    cmd.Parameters.Add(new NpgsqlParameter("job_title", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("salary", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("mgr_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = employee.User_ID;
                    cmd.Parameters[1].Value = employee.Start_Date;
                    cmd.Parameters[2].Value = employee.Job_Title;
                    cmd.Parameters[3].Value = employee.Salary;
                    if(employee.mgr_ID == null) cmd.Parameters[4].Value = DBNull.Value;
                    else cmd.Parameters[4].Value = employee.mgr_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function modifies an Employee record in the database
        public async Task ModifyById(Employee employee)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_ModifyById\"", sql)) // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("user_id", NpgsqlDbType.Integer));
                    cmd.Parameters.Add(new NpgsqlParameter("start_date", NpgsqlDbType.Timestamp));
                    cmd.Parameters.Add(new NpgsqlParameter("job_title", NpgsqlDbType.Varchar));
                    cmd.Parameters.Add(new NpgsqlParameter("salary", NpgsqlDbType.Numeric));
                    cmd.Parameters.Add(new NpgsqlParameter("mgr_id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = employee.User_ID;
                    cmd.Parameters[1].Value = employee.Start_Date;
                    cmd.Parameters[2].Value = employee.Job_Title;
                    cmd.Parameters[3].Value = employee.Salary;
                    cmd.Parameters[4].Value = employee.mgr_ID;
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Function deletes an Emplooyee record in the database
        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_DeleteById\"", sql)) // Specifying stored procedure
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

        // Function returns all employees managed by a specified manager
        public async Task<List<Employee>> getManagedBy(int manager_id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))  // Specifying the database context
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_GetManagedBy\"", sql))   // Specifying stored procedure
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("manager_id", NpgsqlDbType.Integer) { Direction = System.Data.ParameterDirection.Input });
                    cmd.Parameters[0].Value = manager_id;
                    var response = new List<Employee>();
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

        // Mapper used to map between the reader object and our Employee model
        private Employee MapToValue(NpgsqlDataReader reader)
        {

            int? t = null;
            if (!Convert.IsDBNull(reader["mgr_ID"]))
            {
                t = (int?)reader["mgr_ID"];
            }

            return new Employee()
            {
                User_ID = (int)reader["User_ID"],
                Start_Date = (DateTime)reader["Start_Date"],
                Job_Title = reader["Job_Title"].ToString(),
                Salary = (decimal)reader["Salary"],
                mgr_ID = t
            };
        }
    }
}

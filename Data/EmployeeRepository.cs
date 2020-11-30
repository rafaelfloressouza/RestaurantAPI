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

        
        public async Task<List<Employee>> GetAll()
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_GetAll\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Employee>();
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

        public async Task<Employee> GetById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_GetById\"", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    cmd.Parameters[0].Value = id;
                    Employee response = null;
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

        
        public async Task Insert(Employee employee)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_InsertValue\"", sql))
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

        public async Task ModifyById(Employee employee)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_ModifyById\"", sql))
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

        public async Task DeleteById(int id)
        {
            using (NpgsqlConnection sql = new NpgsqlConnection(_connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("\"spEmployee_DeleteById\"", sql))
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

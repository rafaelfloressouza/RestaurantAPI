using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {

        private readonly EmployeeRepository _repository;
        private readonly UserRepository _userRepository;

        public EmployeeController(EmployeeRepository repository, UserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(repository));
        }

        
        // GET: api/employee
        [HttpGet]
        public async Task<List<Employee>> Get()
        {
            // Getting all records from the Employee table
            return await _repository.GetAll();
        }


        // GET api/employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return NotFound("Employee record you are searching for does not exist or URL is wrong");
            }
        }


        // POST api/employee
        [HttpPost]
        public ActionResult Post()
        {
            return BadRequest("ERROR: You cannot insert entries into the Employee table. Try inserting a new user");
        }

        // PUT api/employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Employee employee)
        {
            // If id in body does not match id in URL
            if (id != employee.User_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(id);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Employee record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(employee);
                    string format = "Employee record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }

            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Employee record scould not be updated\n");
            }
        }

        // DELETE api/employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record in the Employee table
                var response = await _repository.GetById(id);

                // Deleting record from User table (it will cascade to the Employee table)
                await _userRepository.DeleteById(id);
                string format = "Employee record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, id));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Employee record could not be deleted\n");
            }
        }

        //api/employee/getManagedBy
        [Route("getManagedBy/{manager_id}")]
        [HttpGet]
        public async Task<List<Employee>> getManagedBy(int manager_id)
        {
            // Getting all employees managed by manager with user_id
            return await _repository.getManagedBy(manager_id);
        }
    }
}

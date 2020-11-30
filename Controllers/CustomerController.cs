using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private readonly CustomerRepository _repository;
        private readonly UserRepository _userRepository;

        public CustomerController(CustomerRepository repository, UserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
        }

        // GET: api/customer
        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            // Getting all records from the Customer table
            return await _repository.GetAll();
        }

        // GET api/customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
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
                return NotFound("Record you are searching for does not exist");
            }
        }

        // POST api/customer
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Customer table. It has to be done directly through the user table
            return BadRequest("ERROR: You cannot insert entries into the Customer table. Try inserting a new user\n");
        }

        // PUT api/customer/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Customer customer)
        {
            // If id in body does not match id in URL
            if (id != customer.User_ID)
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
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(customer);
                    string format = "The record with key={0} was updated succesfully\n";
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
                return BadRequest("Error: Record scould not be updated\n");
            }
        }

        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record inn the Customer table
                var response = await _repository.GetById(id);

                // Deleting record from User table (it will cascade to the Customer table)
                await _userRepository.DeleteById(id);
                string format = "Record with key={0} deleted succesfully\n";
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
                return BadRequest("Error: Record could not be deleted\n");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;


namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class WaiterController : Controller
    {
        private readonly WaiterRepository _repository;
        private readonly UserRepository _userRepository;

        public WaiterController(WaiterRepository repository, UserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
        }

        // GET: api/waiter
        [HttpGet]
        public async Task<List<Waiter>> Get()
        {
            // Getting all records from the Waiter table
            return await _repository.GetAll();
        }

        // GET api/waiters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Waiter>> Get(int id)
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
                return NotFound("Waiter record you are searching for does not exist or URL is wrong");
            }
        }

        // POST api/waiter
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Waiter table. It has to be done directly through the user table
            return BadRequest("ERROR: You cannot insert entries into the Waiter table. Try inserting a new user\n");
        }

        // PUT api/waiter/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Waiter waiter)
        {
            // If id in body does not match id in URL
            if (id != waiter.User_ID)
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
                    return NotFound("Waiter record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(waiter);
                    string format = "Waiter record with key={0} was updated succesfully\n";
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
                return BadRequest("Error: Waiter record scould not be updated\n");
            }
        }

        // DELETE api/waiter/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record inn the Customer table
                var response = await _repository.GetById(id);

                // Deleting record from User table (it will cascade to the Customer table)
                await _userRepository.DeleteById(id);
                string format = "Waiter record with key={0} deleted succesfully\n";
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
                return BadRequest("Error: Waiter record could not be deleted\n");
            }
        }
    }
}

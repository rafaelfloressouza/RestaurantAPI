using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ManagerRepository _repository;
        private readonly UserRepository _userRepository;

        public ManagerController(ManagerRepository repository, UserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(repository));
        }

        // GET: api/manager
        [Route("api/[controller]")]
        [HttpGet]
        public async Task<List<Manager>> Get()
        {
            // Getting all records from the Manager table
            return await _repository.GetAll();
        }


        // GET api/manager/5
        [Route("api/[controller]/{id:int}")]
        [HttpGet]
        public async Task<ActionResult<Manager>> Get(int id)
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


        // POST api/manager
        [Route("api/[controller]")]
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Manager table. It has to be done directly through the user table
            return BadRequest("ERROR: You cannot insert entries into the Manager table. Try inserting a new user\n");
        }


        // PUT api/manager/5
        [Route("api/[controller]/{id:int}")]
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Manager manager)
        {
            // If id in body does not match id in URL
            if (id != manager.User_ID)
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
                    await _repository.ModifyById(manager);
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
                return BadRequest("Error: Record could not be updated\n");
            }
        }

        // DELETE api/manager/5
        [Route("api/[controller]/{id:int}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record inn the Manager table
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

        //api/manager/getNumberManagers
        [Route("api/[controller]/getNumberManagers")]
        [HttpGet]
        public async Task<ActionResult> getManagerNumber()
        {
            try
            {
                int response = await _repository.getManagerNum();
                string format = "There are {0} manager(s)\n";
                return Ok(String.Format(format, response));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Number of managers could not be retrievedn");
            }
        }
    }
}

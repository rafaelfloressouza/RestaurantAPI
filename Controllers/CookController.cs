using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class CookController : Controller
    {
        private readonly CookRepository _repository;
        private readonly UserRepository _userRepository;

        public CookController(CookRepository repository, UserRepository userRepo)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        }

        // GET: api/cook
        [HttpGet]
        public async Task<List<Cook>> Get()
        {
            // Get all records from the cook table
            return await _repository.GetAll();
        }


        // GET api/cook/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cook>> Get(int id)
        {
            try
            {
                // Searching for record
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                // Posgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return NotFound("Record you are searching for does not exist");
            }
        }


        // POST api/cook
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Cook table. It has to be done directly through the user table
            return BadRequest("ERROR: You cannot insert entries into the Cook table. Try inserting a new user\n");
        }


        // PUT api/cook/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Cook cook)
        {
            if (id != cook.User_ID)                           
            {
                // If id from body and id from URL don't match
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for reacoord
                var response = await _repository.GetById(id); 
                    
                if (response == null)                         
                {
                    // If record does not exist
                    return NotFound("Record was not found\n");
                }
                else                                         
                {
                    // Recornd exists, then modify it
                    await _repository.ModifyById(cook);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }
            }
            catch (Npgsql.PostgresException ex)              
            {
                // Postgres threw some exception
                return BadRequest(ex.Message.ToString());
            }
            catch                                          
            {
                // Unknown error
                return BadRequest("Error: Record could not be updated\n");
            }
        }

        // DELETE api/cook/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Search if the record exists
                var response = await _repository.GetById(id);

                // We delete the user (it will cascade to the cook)
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

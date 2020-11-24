using System;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _repository;
        private readonly CustomerRepository _customerRepository;

        public UserController(UserRepository repository, CustomerRepository customerRepository) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        // GET: api/user
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _repository.GetAll();
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                return NotFound("Record you are searching for does not exist");
            }
        }

        // POST api/user/customer
        [HttpPost("{category}")]
        public async Task<ActionResult> Post([FromBody] User user, string category)
        {
            category = category.ToLower();

            //try
            //{
                if (category == "customer")
                {
                    await _repository.Insert(user);
                    //var last_user = _repository.getLastInserted();
                    //await _customerRepository.Insert(new Customer{User_ID = (int)last_user, TableNo = null});  // Inserting new user into customer table
                }
                else
                {
                    return BadRequest("Error: Insert a valid category of user\n");
                }

                return Ok("Record inserted successfully\n");
            //}
            //catch (Npgsql.PostgresException ex)
            //{
            //    return BadRequest(ex.Message.ToString());
            //}
            //catch
            //{
            //    return BadRequest("Error: Record was not inserted\n");
            //}

        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] User user)
        {
            if (id != user.ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                var response = await _repository.GetById(id);

                if (response == null)
                {
                    return NotFound("Record was not found\n");
                }
                else
                {
                    await _repository.ModifyById(user);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }

            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                return BadRequest("Error: Record could not be updated\n");
            }
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _repository.GetById(id);
                await _repository.DeleteById(id);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, id));
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                return BadRequest("Error: Record could not be deleted\n");
            }
        }
    }
}

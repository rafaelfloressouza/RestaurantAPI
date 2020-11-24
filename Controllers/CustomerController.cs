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

        public CustomerController(CustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/customer
        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            return await _repository.GetAll();
        }

        // GET api/customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
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

        // POST api/customer
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                await _repository.Insert(customer);
                return Ok("Record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                return BadRequest("Error: Record was not inserted\n");
            }
        }

        // PUT api/customer/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Customer customer)
        {
            if (id != customer.User_ID)
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
                    await _repository.ModifyById(customer);
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

        // DELETE api/customer/5
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

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {

        private readonly OrderRepository _repository;
      
        public OrderController(OrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/order
        [HttpGet]
        public async Task<List<Order>> Get()
        {
            // Getting all records from the Order table
            return await _repository.GetAll();
        }

        // GET api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
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

        // POST api/order
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order order)
        {
            try
            {
                // Inserting record in the Order table
                await _repository.Insert(order);
                return Ok("Record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Record was not inserted\n");
            }
        }

        // PUT api/order/5
        [HttpPut("{order_id}")]
        public async Task<ActionResult> Put(int order_id, [FromBody] Order order)
        {
            // If id in body does not match id in URL
            if (order_id != order.Order_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(order_id);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(order);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, order_id));
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

        // DELETE api/order/5
        [HttpDelete("{order_id}")]
        public async Task<ActionResult> Delete(int order_id)
        {
            try
            {
                // Searching for record in the Order table
                var response = await _repository.GetById(order_id);

                // Deleting record from Order table
                await _repository.DeleteById(order_id);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, order_id));
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

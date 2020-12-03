using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Online_OrderController : Controller
    {
        private readonly Online_OrderRepository _repository;
        private readonly OrderRepository _orderRepository;
       
        public Online_OrderController(Online_OrderRepository repository, OrderRepository orderRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        // GET: api/online_order
        [HttpGet]
        public async Task<List<Online_Order>> Get()
        {
            // Get all records from the Online_Order table
            return await _repository.GetAll();
        }


        // GET api/online_order/5
        [HttpGet("{order_id}")]
        public async Task<ActionResult<Online_Order>> Get(int order_id)
        {
            try
            {
                // Searching for record
                var response = await _repository.GetById(order_id);
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


        // POST api/online_order
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Online_Order table. It has to be done directly through the Order table
            return BadRequest("ERROR: You cannot insert entries into the Online_Order table. Try inserting a new Order\n");
        }


        // PUT api/online_order/5
        [HttpPut("{order_id}")]
        public async Task<ActionResult> Put(int order_id, [FromBody] Online_Order online_order)
        {
            if (order_id  != online_order.Order_ID)
            {
                // If id from body and id from URL don't match
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for reacoord
                var response = await _repository.GetById(order_id);

                if (response == null)
                {
                    // If record does not exist
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // Recornd exists, then modify it
                    await _repository.ModifyById(online_order);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, order_id));
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

        // DELETE api/online_order/5
        [HttpDelete("{order_id}")]
        public async Task<ActionResult> Delete(int order_id)
        {
            try
            {
                // Search if the record exists
                var response = await _repository.GetById(order_id);

                // We delete the order (it will cascade to the online_order)
                await _orderRepository.DeleteById(order_id);
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

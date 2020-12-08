using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class In_Store_OrderController : Controller
    {
        private readonly In_Store_OrderRepository _repository;
        private readonly OrderRepository _orderRepository;

        public In_Store_OrderController(In_Store_OrderRepository repository, OrderRepository orderRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        // GET: api/in_store_order
        [HttpGet]
        public async Task<List<In_Store_Order>> Get()
        {
            // Get all records from the In_Store_Order table
            return await _repository.GetAll();
        }


        // GET api/in_store_order/5
        [HttpGet("{order_id}")]
        public async Task<ActionResult<In_Store_Order>> Get(int order_id)
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
                return NotFound("In_Store_Order record you are searching for does not exist or the URL is wrong");
            }
        }


        // POST api/in_store_order
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the In_Store_Order table. It has to be done directly through the Order table
            return BadRequest("ERROR: You cannot insert entries into the In_Store_Order table. Try inserting a new Order\n");
        }


        // PUT api/in_store_order/5
        [HttpPut("{order_id}")]
        public async Task<ActionResult> Put(int order_id, [FromBody] In_Store_Order in_store_order)
        {
            if (order_id != in_store_order.Order_ID)
            {
                // If id from body and id from URL don't match
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record
                var response = await _repository.GetById(order_id);

                if (response == null)
                {
                    // If record does not exist
                    return NotFound("In_Store_Order record was not found\n");
                }
                else
                {
                    // Record exists, then modify it
                    await _repository.ModifyById(in_store_order);
                    string format = "In_Store_Order record with key={0} was updated succesfully\n";
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
                return BadRequest("Error: In_Store_Order record could not be updated\n");
            }
        }

        // DELETE api/in_store_order/5
        [HttpDelete("{order_id}")]
        public async Task<ActionResult> Delete(int order_id)
        {
            try
            {
                // Search if the record exists
                var response = await _repository.GetById(order_id);

                // We delete the order (it will cascade to the in_store_order)
                await _orderRepository.DeleteById(order_id);
                string format = "In_Store_Order record with key={0} deleted succesfully\n";
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
                return BadRequest("Error: In_Store_Order record could not be deleted\n");
            }
        }

        // api/in_store_order/getOrdersByWaiter/5
        [Route("getOrdersByWaiter/{waiter_id}")]
        [HttpGet]
        public async Task<List<In_Store_Order>> getOrdersByWaiter(int waiter_id)
        {
            try {

                // Returning all in-store-order received by the specified waiter
                return await _repository.getOrdersByWaiter(waiter_id);
            }catch{
                return new List<In_Store_Order>();
            }
        }

        // api/in_store_order/getOrdersByTable/5
        [Route("getOrdersByTable/{tableno}")]
        [HttpGet]
        public async Task<List<In_Store_Order>> getOrdersByTable(int tableno)
        {
            try
            {
                // Returning all in-store-order received by the specified table
                return await _repository.getOrdersByTable(tableno);
            }
            catch
            {
                return new List<In_Store_Order>();
            }
        }
    }
}

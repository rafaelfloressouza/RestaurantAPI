using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;
using System.Globalization;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Order_TableController : Controller
    {

        private readonly Order_TableRepository _repository;
        private readonly OrderRepository _orderRepository;
        
        public Order_TableController(Order_TableRepository repository, OrderRepository orderRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        // GET: api/order_table
        [HttpGet]
        public async Task<List<Order_Table>> Get()
        {
            // Getting all records from the Order_Table table
            return await _repository.GetAll();
        }

        // GET api/order_table/4/5
        [HttpGet("{order_id}/{tableno}")]
        public async Task<ActionResult<Order_Table>> Get(int order_id, int tableno)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(order_id, tableno);
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

        // POST api/order_table
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order_Table order_table)
        {
            try
            {
                //If we are trying to insert and order that is already comming from a table
                if (await _repository.orderExists(order_table.Order_ID))
                {
                    return BadRequest("ERROR: That order is already linked to a table. An order can only come from one table");
                }

                // Inserting record in the Order_Table table
                await _repository.Insert(order_table);
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

        // PUT api/order_table
        [HttpPut]
        public ActionResult Put()
        {
            // We cannot modify entries in the Dish_Ingredient table. It has to be done directly through deletes and posts
            return BadRequest("ERROR: You cannot modify entries in the Order_Table table. Try using POST and DELETE instead.\n");
        }

        // DELETE api/order_table/2/4
        [HttpDelete("{order_id}/{tableno}")]
        public async Task<ActionResult> Delete(int order_id, int tableno)
        {
            try
            {
                // Searching for record in the Order_Table table
                var response = await _repository.GetById(order_id, tableno);

                // Deleting record from Order table (it will cascade to In-Store_Order and Order_Table
                await _orderRepository.DeleteById(order_id);
                string format = "The Order with id={0} was deleted from the Order, In-Store_Order and Order_Table tables\n";
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

        // GET api/order_table/orderExists/4
        [Route("orderExists/{order_id}")]
        [HttpGet]
        public async Task<ActionResult> getNumSuppliers(int order_id)
        {
            try
            {
                string format = "The order with id={0} {1}\n";
                if (await _repository.orderExists(order_id))
                {
                    return Ok(string.Format(format, order_id, "exists"));
                }
                else
                {
                    return Ok(string.Format(format, order_id, "does not exist"));
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Some unknown exception
                return BadRequest("ERROR: Number of orders for that record could not be retrieved");
            }
        }
    }
}

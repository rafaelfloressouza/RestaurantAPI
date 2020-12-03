using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Order_DishController : Controller
    {

        private readonly Order_DishRepository _repository;
        private readonly OrderRepository _orderRepository;
        private readonly TransactionRepository _transationRepository;
    
        public Order_DishController(Order_DishRepository repository, OrderRepository orderRepository, TransactionRepository transactionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _transationRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        // GET: api/order_dish
        [HttpGet]
        public async Task<List<Order_Dish>> Get()
        {
            // Getting all records from the Order_Dish table
            return await _repository.GetAll();
        }

        // GET api/order_dish/3/4
        [HttpGet("{order_id}/{dish_id}")]
        public async Task<ActionResult<Order_Dish>> Get(int order_id, int dish_id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(order_id, dish_id);
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

        // POST api/order_dish
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order_Dish order_dish)
        {
            try
            {
                // Inserting record in the Order_Dish table
                await _repository.Insert(order_dish);
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

        // PUT api/order_dish
        [HttpPut]
        public ActionResult Put()
        {
            // We cannot modify entries in the Order_Dish table. It has to be done directly through deletes and posts
            return BadRequest("ERROR: You cannot modify entries in the Order_Dish table. Try using POST and DELETE instead.\n");
        }

        // DELETE api/order_dish/4/5
        [HttpDelete("{order_id}/{dish_id}")]
        public async Task<ActionResult> Delete(int order_id, int dish_id)
        {
            try
            {
                // Searching for record in the Order_Dish table
                var response = await _repository.GetById(order_id, dish_id);

                string format1 = "Record in the Order_Dish table with key=(Dish_ID={0},Order_ID={1}) deleted succesfully\n";
                string format2 = "Record in the Order table with Order_ID={0} deleted because orders should contain at least one dish (the last dish was removed)\n";
                
                // Getting number of dishes in the order for that ingredient
                if (await _repository.getNumberOfDishes(order_id) == 1)
                {
                    // Deleting record from Order_Dish table and Order
                    // Due to foreign key constraints we can simply delete the order from the Order table
                    var order_response = await _orderRepository.GetById(order_id);
                    await _orderRepository.DeleteById(order_id);

                    // In case we delete the last order contained in a transaction, we delete the transaction as well
                    if(await _orderRepository.numOrderByTransaction(order_response.Transaction_ID) == 1)
                    {
                        await _transationRepository.DeleteById(order_response.Transaction_ID);
                        return Ok(string.Format("Records in the Order, Order_Dish due to database constraints\n"));
                    }

                    return Ok(string.Format(format2, order_id));
                }
                else
                {
                    // Deleting record from Order_Dish table
                    await _repository.DeleteById(order_id, dish_id);
                    return Ok(string.Format(format1, dish_id, order_id));
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
                return BadRequest("Error: Record could not be deleted\n");
            }
        }

        [Route("getNumDishes/{order_id}")]
        [HttpGet]
        public async Task<ActionResult> getNumDishes(int order_id)
        {
            try
            {
                // There is no error and we are able to retrieve the number of dishes for the specified order
                string format = "The number of dishes in order={0} is {1}\n";
                return Ok(string.Format(format, order_id, await _repository.getNumberOfDishes(order_id)));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Some unknown exception
                return BadRequest("ERROR: Number of dishes for that record could not be retrieved");
            }
        }
    }
}

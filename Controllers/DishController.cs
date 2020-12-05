using System;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class DishController : Controller
    {
        private readonly DishRepository _repository;
        private readonly Order_DishRepository _orderDishRepository;
        private readonly OrderRepository _orderRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly IngredientRepository _ingredientRepository;
        private readonly Dish_IngredientRepository _dishIngredientRepository;

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public DishController(DishRepository repository, Order_DishRepository orderDishRepository, OrderRepository orderRepository, TransactionRepository transactionRepository, IngredientRepository ingredientRepository, Dish_IngredientRepository dishIngredientRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderDishRepository = orderDishRepository ?? throw new ArgumentNullException(nameof(orderDishRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _ingredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository));
            _dishIngredientRepository = dishIngredientRepository ?? throw new ArgumentException(nameof(dishIngredientRepository));
        }

        // GET: api/dish
        [HttpGet]
        public async Task<List<Dish>> Get()
        {
            // Get all records from the Dish table 
            return await _repository.GetAll();
        }


        // GET api/dish/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> Get(int id)
        {
            try
            {
                // Searching for record in the Dish table
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

        // POST api/dish/potato
        [HttpPost("{potato}")]
        public async Task<ActionResult> Post([FromBody] Dish dish, string ing_name)
        {
            // Converting ingredient name to title case (for convention)
            ing_name = textInfo.ToTitleCase(ing_name.ToLower());

            try
            {
                // Making sure ingredient exists. If it does not, this will throw an exception
                await _ingredientRepository.GetByName(ing_name);

                // Inserting record in the Dish table
                await _repository.Insert(dish);

                int last_inserted_dish = await _repository.getLastInserted();

                // Inserting record in the Dish_Ingredient table
                await _dishIngredientRepository.Insert(new Dish_Ingredient { Dish_ID = last_inserted_dish, Ing_Name = ing_name });

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
                return BadRequest("Error: Record was not inserted. Make sure you are providing an ingredient that exists\n");
            }
        }

        // PUT api/dish/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Dish dish)
        {

            // If id in body does not match the id in the URL
            if (id != dish.Dish_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the Dish table
                var response = await _repository.GetById(id);

                // If record does not exists
                if (response == null)
                {
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // Record exists then modify its
                    await _repository.ModifyById(dish);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw and exception
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Record could not be updated\n");
            }
        }

        // DELETE api/dish/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record in the Dish table (if its not found an exception is thrown)
                var response = await _repository.GetById(id);

                var order_list = await _orderDishRepository.getOrderList(id);

                foreach(Order order in order_list)  // Orders that contain the dish to be deleted
                {
                    // If the removed dish is the only one contained in an order, then delete the order as well
                    if(await _orderDishRepository.getNumberOfDishes(order.Order_ID) == 1)
                    {

                        // It is the last dish contained in this order
                        // Remove the Order, but check if it is the last order in the transaction
                        if (await _orderRepository.numOrderByTransaction(order.Transaction_ID) == 1) // If the order is the only one in the transaction
                        {
                            // Removinng both the order and transaction 
                            await _orderRepository.DeleteById(order.Order_ID);
                            await _transactionRepository.DeleteById(order.Transaction_ID);
                            // NOTE Elements in the Order_Dish table will also be deleted (cascading) which is what we want.
                        }
                        else // Transaction contains more than 1 orders still
                        {
                            await _orderRepository.DeleteById(order.Order_ID);
         
                            // Updating the transaction
                            await _transactionRepository.updateAmount(order.Transaction_ID, await _transactionRepository.getAmount(order.Transaction_ID) - response.Price);
                            // NOTE Elemets in the Order_Dish table will also be deleted (cascading) which is what we want.
                        }
                    }
                    else
                    {
                        // If it is not the last dish we do the following
                        // Update the transaction related to the order (which the dish is in)
                        await _transactionRepository.updateAmount(order.Transaction_ID, await _transactionRepository.getAmount(order.Transaction_ID) - response.Price);
                    }
                }

                // After validating the database constraints, delete the dish
                await _repository.DeleteById(id);

                string format = "Record with key={0} and related records deleted succesfully due to database constraints\n";
                return Ok(string.Format(format, id));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exceptions
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                // Unknown errors
                return BadRequest("Error: Record could not be deleted\n");
            }
        }

        // api/dish/getLastInserted
        [Route("getLastInserted")]
        [HttpGet]
        public async Task<ActionResult> getLastInserted()
        {
            try
            {
                // Getting the id of the last inserted dish
                string format = "The last inserted dish has id={0}\n";
                return Ok(string.Format(format, await _repository.getLastInserted()));
            }
            catch
            {
                // Some unknown error occurred
                return BadRequest("ERROR: Unable to get the id of the last inserted dish\n");
            }
        }

        //api/dish/getIngredients/5
        [Route("getIngredients/{dish_id}")]
        [HttpGet]
        public async Task<List<string>> getIngredients(int dish_id)
        {
            // Getting all ingredients for a specific dish 
            return await _repository.getIngredients(dish_id);
        }
    }    
}

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
        private readonly UserRepository _userRepository;
        private readonly TableRepository _tableRepository;

        public CustomerController(CustomerRepository repository, UserRepository userRepository, TableRepository tableRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
            _tableRepository = tableRepository ?? throw new ArgumentException(nameof(tableRepository));
        }

        // GET: api/customer
        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            // Getting all records from the Customer table
            return await _repository.GetAll();
        }

        // GET api/customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
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
                return NotFound("Record you are searching for does not exist or the URI is wrong\n");
            }
        }

        // POST api/customer
        [HttpPost]
        public ActionResult Post()
        {
            // We cannot add any entry directly to the Customer table. It has to be done directly through the user table
            return BadRequest("ERROR: You cannot insert entries into the Customer table. Try inserting a new user\n");
        }

        //api/customer/sit/3/5
        [Route("sit/{user_id}/{tableno}")]
        [HttpPut]
        public async Task<ActionResult> Sit(int user_id, int tableno)
        {
            try
            {
                // Making sure customer and table exist
                await _tableRepository.GetById(tableno);
                await _repository.GetById(user_id);

                // "Sitting" the customer at a table
                await _repository.Sit(user_id, tableno);

                // Making the table occupied
                await _tableRepository.makeOcuppied(tableno);

                string format = "The customer with id={0} is sitting at table {1}";
                return Ok(string.Format(format, user_id, tableno));
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

        //api/customer/leave/6
        [Route("leave/{user_id}")]
        [HttpPut]
        public async Task<ActionResult> Leave(int user_id)
        {
            try
            {
                // Making sure customer exists
                Customer customer = await _repository.GetById(user_id);

                // Customer leaves the table
                await _repository.Leave(user_id);

                // Making the table available
                await _tableRepository.makeDisoccupied(customer.TableNo);

                string format = "The customer with id={0} left table {1}";
                return Ok(string.Format(format, user_id, customer.TableNo));
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

        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record inn the Customer table
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

        //api/customer/AtTable/5
        [Route("AtTable/{tableno}")]
        [HttpGet]
        public async Task<List<Customer>> atTable(int tableno)
        {
            // Getting all customer currently sitting at a table 
            return await _repository.atTable(tableno);
        }

        //api/customer/getTransactions/5
        [Route("getTransactions/{user_id}")]
        [HttpGet]
        public async Task<List<Transaction>> getTransactions(int user_id)
        {
            // Getting all trasactions for a specific customer 
            return await _repository.getTransactions(user_id);
        }

        //api/customer/getReviews/5
        [Route("getReviews/{user_id}")]
        [HttpGet]
        public async Task<List<Review>> getReviews(int user_id)
        {
            // Getting all trasactions for a specific customer 
            return await _repository.getReviews(user_id);
        }

        //api/customer/getOrders/5
        [Route("getOrders/{user_id}")]
        [HttpGet]
        public async Task<List<Order>> getOrders(int user_id)
        {
            // Getting all dishes for a specific order 
            return await _repository.getOrders(user_id);
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Customer_TransactionController : Controller
    {

        private readonly Customer_TransactionRepository _repository;
        private readonly CustomerRepository _customerRepository;
        private readonly TransactionRepository _transactionRepository;

        public Customer_TransactionController(Customer_TransactionRepository repository, CustomerRepository customerRepository, TransactionRepository transactionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        // GET: api/customer_transaction
        [HttpGet]
        public async Task<List<Customer_Transaction>> Get()
        {
            // Getting all records from the Customer_Transaction table
            return await _repository.GetAll();
        }

        // GET api/customer_transaction/5/10
        [HttpGet("{user_id}/{tran_id}")]
        public async Task<ActionResult<Customer_Transaction>> Get(int user_id, int tran_id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(user_id, tran_id);
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
                return NotFound("Customer_Transaction Record you are searching for does not exist or URL is wrong\n");
            }
        }

        // POST api/customer_transaction
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customer_Transaction customer_transaction)
        {
            try
            {
                // Making sure that the customer being referenced exists
                await _customerRepository.GetById(customer_transaction.User_ID);
                await _transactionRepository.GetById(customer_transaction.Transaction_ID);
                
                // Inserting record in the Customer_Transaction table
                await _repository.Insert(customer_transaction);
                return Ok("Customer_Transaction Record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Customer_Transaction record was not inserted. Make sure the transactioon and customer exist.\n");
            }
        }

        // PUT api/customer_transaction
        [HttpPut]
        public ActionResult Put()
        {
            // We cannot add any modify entries in the Customer_Transaction table. It has to be done directly through deletes and posts
            return BadRequest("ERROR: You cannot modify entries int the Customer_Transaction table. Try using POST and DELETE instead.\n");
        }

        // DELETE api/customer_transaction/5/10
        [HttpDelete("{user_id}/{tran_id}")]
        public async Task<ActionResult> Delete(int user_id, int tran_id)
        {
            try
            {
                // Searching for record inn the Customer_Transaction table
                var response = await _repository.GetById(user_id, tran_id);

                // Deleting record from Customer_Transactionn table
                await _repository.DeleteById(user_id, tran_id);
                string format = "Customer_Transaction record with key=({0},{1}) deleted succesfully\n";
                return Ok(string.Format(format, user_id, tran_id));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Customer_Transaction record could not be deleted\n");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Order_TransactionController : Controller
    {

        private readonly Order_TransactionRepository _repository;
        private readonly TransactionRepository _transactionRepository;

        public Order_TransactionController(Order_TransactionRepository repository, TransactionRepository transactionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        // GET: api/order_transaction
        [HttpGet]
        public async Task<List<Order_Transaction>> Get()
        {
            // Getting all records from the Order_Transaction table
            return await _repository.GetAll();
        }

        // GET api/order_transaction/4/5
        [HttpGet("{order_id}/{tran_id}")]
        public async Task<ActionResult<Order_Transaction>> Get(int order_id, int tran_id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(order_id, tran_id);
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

        // POST api/order_transaction
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order_Transaction order_transaction)
        {
            try
            {
                // Inserting record in the Order_Table table
                await _repository.Insert(order_transaction);
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

        // PUT api/order_transaction
        [HttpPut]
        public ActionResult Put()
        {
            // We cannot modify entries in the order_transaction table. It has to be done directly through deletes and posts
            return BadRequest("ERROR: You cannot modify entries in the Order_Transaction table. Try using POST and DELETE instead.\n");
        }

        // DELETE api/order_transaction/3/4
        [HttpDelete("{order_id}/{tran_id}")]
        public async Task<ActionResult> Delete(int order_id, int tran_id)
        {
            try
            {
                // Searching for record in the Order_Transaction table
                var response = await _repository.GetById(order_id, tran_id);

                string format1 = "The Transaction with id={0} was deleted from the Transaction and Order_Transaction tables\n";
                string format2 = "The record with key=(Order_ID={0},Transaction_ID={1}) was deleted from the Order_Transaction table\n";

                // There is a single order in the transaction (erase the transaction)
                if (await _repository.getNumOrders(tran_id) == 1)
                {
                    // Deleting record from Transaction table (it will cascade to Order_Transaction Table)
                    await _transactionRepository.DeleteById(tran_id);
                    return Ok(string.Format(format1, tran_id));
                }
                else
                {
                    // There is more than one record
                    await _repository.DeleteById(order_id, tran_id);
                    return Ok(string.Format(format2, order_id, tran_id));
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

        // GET api/order_transaction/getNumOrders/4
        [Route("getNumOrders/{tran_id}")]
        [HttpGet]
        public async Task<ActionResult> getNumSuppliers(int tran_id)
        {
            try
            {
                // Getting the number of order in the specified transaction
                string format = "The number of orders in transaction id={0} is {1}\n";
                return Ok(string.Format(format, tran_id, await _repository.getNumOrders(tran_id)));
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

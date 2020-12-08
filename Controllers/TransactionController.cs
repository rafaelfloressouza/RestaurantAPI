using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {

        private readonly TransactionRepository _repository;

        public TransactionController(TransactionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/transaction
        [HttpGet]
        public async Task<List<Transaction>> Get()
        {
            // Getting all records from the Transaction table
            return await _repository.GetAll();
        }

        // GET api/transactions/5
        [HttpGet("{tran_id}")]
        public async Task<ActionResult<Transaction>> Get(int tran_id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(tran_id);
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
                return NotFound("Transaction record you are searching for does not exist or the URL is wrong");
            }
        }

        // POST api/transaction
        [HttpPost]
        public ActionResult Post()
        {
            return BadRequest("Error: Records cannot be added to the Trasaction Table. Try inserting and Order and a Transaction will be automatically be created\n");
        }

        // PUT api/transaction/5
        [HttpPut("{tran_id}")]
        public async Task<ActionResult> Put(int tran_id, [FromBody] Transaction transaction)
        {
            // If id in body does not match id in URL
            if (tran_id != transaction.Transaction_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(tran_id);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Transaction record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(transaction);
                    string format = "Transaction record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, tran_id));
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
                return BadRequest("Error: Transaction record scould not be updated\n");
            }
        }

        // DELETE api/transaction/5
        [HttpDelete("{tran_id}")]
        public async Task<ActionResult> Delete(int tran_id)
        {
            try
            {
                // Searching for record in the Transaction table
                var response = await _repository.GetById(tran_id);

                // Deleting record from Transaction table
                await _repository.DeleteById(tran_id);
                string format = "Transaction record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, tran_id));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Transaction record could not be deleted\n");
            }
        }

        // api/transaction/4
        [Route("getAmount/{tran_id}")]
        [HttpGet]
        public async Task<ActionResult> getAmount(int tran_id)
        {
            try
            {
                // Getting the amount in dollars of the transaction
                string format = "The amount of dollars for the transaction with id={0} is ${1}\n";
                return Ok(string.Format(format, tran_id, await _repository.getAmount(tran_id)));
            }
            catch 
            {
                // Some unknown error occurred
                return BadRequest("ERROR: The dollar amount of the transaction could not be retrieved\n");
            }
        }
    }
}

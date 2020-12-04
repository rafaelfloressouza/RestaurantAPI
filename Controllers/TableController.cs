using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class TableController : Controller
    {

        private readonly TableRepository _repository;
        
        public TableController(TableRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/table
        [HttpGet]
        public async Task<List<Table>> Get()
        {
            // Getting all records from the Table table
            return await _repository.GetAll();
        }

        // GET api/table/5
        [HttpGet("{tableno}")]
        public async Task<ActionResult<Table>> Get(int tableno)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(tableno);
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

        // POST api/table
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Table table)
        {
            try
            {
                // Inserting record in the Table table
                await _repository.Insert(table);
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

        // PUT api/table/5
        [HttpPut("{tableno}")]
        public async Task<ActionResult> Put(int tableno, [FromBody] Table table)
        {
            // If id in body does not match id in URL
            if (tableno != table.TableNo)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(tableno);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(table);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, tableno));
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

        // DELETE api/table/5
        [HttpDelete("{tableno}")]
        public async Task<ActionResult> Delete(int tableno)
        {
            try
            {
                // Searching for record in the Table table
                var response = await _repository.GetById(tableno);

                // Deleting record from Table table
                await _repository.DeleteById(tableno);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, tableno));
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

        // GET: api/table/getWaitedBy/4
        [Route("getWaitedBy/{waiter_id}")]
        [HttpGet]
        public async Task<List<Table>> getWaitedBy(int waiter_id)
        {
            // Getting all tables waited by the specified waiter
            return await _repository.getWaitedBy(waiter_id);
        }
    }
}

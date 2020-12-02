using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {

        private readonly ReviewRepository _repository;

        public ReviewController(ReviewRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/review
        [HttpGet]
        public async Task<List<Review>> Get()
        {
            // Getting all records from the Review table
            return await _repository.GetAll();
        }

        // GET api/review/5/6
        [HttpGet("{user_id}/{review_id}")]
        public async Task<ActionResult<Review>> Get(int user_id, int review_id)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(user_id, review_id);
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

        // POST api/review
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Review review)
        {
            try
            {
                // Inserting record in the Review table
                await _repository.Insert(review);
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

        // PUT api/review/5/6
        [HttpPut("{user_id}/{review_id}")]
        public async Task<ActionResult> Put(int user_id, int review_id, [FromBody] Review review)
        {
            // If id in body does not match id in URL
            if (user_id != review.User_ID)
            {
                return BadRequest("user_id in URL has to match the user_id of the record to be updated\n");
            }

            if (review_id != review.Review_ID)
            {
                return BadRequest("review_id in URL has to match the review_id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(user_id, review_id);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(review);
                    string format = "The record with key={0},{1} was updated succesfully\n";
                    return Ok(String.Format(format, user_id, review_id));
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

        // DELETE api/review/5/6
        [HttpDelete("{user_id}/{review_id}")]
        public async Task<ActionResult> Delete(int user_id, int review_id)
        {
            try
            {
                // Searching for record inn the Review table
                var response = await _repository.GetById(user_id, review_id);

                // Deleting record from Review table
                await _repository.DeleteById(user_id, review_id);
                string format = "Record with key={0},{1} deleted succesfully\n";
                return Ok(string.Format(format, user_id, review_id));
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

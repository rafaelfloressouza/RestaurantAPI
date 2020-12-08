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
        private readonly UserRepository _userRepository;

        public ReviewController(ReviewRepository repository, UserRepository userRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
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
                return NotFound("Review record you are searching for does not exist or the URL is wrong");
            }
        }

        // POST api/review
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Review review)
        {
            try
            {
                // Making sure referenced user exists
                await _userRepository.GetById(review.User_ID);

                // Getting correct key for the review (a Key that keeps Review as a weak entity)
                review.Review_ID = await _repository.getNextReviewID(review.User_ID);

                // Inserting record in the Review table
                await _repository.Insert(review);
                return Ok("Review record inserted successfully\n");
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
                    return NotFound("Review record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(review);
                    string format = "Review record with key=({0},{1}) was updated succesfully\n";
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
                return BadRequest("Error: Review record could not be updated\n");
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
                string format = "Review record with key=({0},{1}) deleted succesfully\n";
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
                return BadRequest("Error: Review record could not be deleted\n");
            }
        }
    }
}

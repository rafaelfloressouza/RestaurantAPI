using System;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class DishController : Controller
    {

        private readonly DishRepository _repository;

        public DishController(DishRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/dish
        [HttpGet]
        public async Task<List<Dish>> Get()
        {
            return await _repository.GetAll();
        }

        
        // GET api/dish/5
          [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> Get(int id)
        {
            try
            {
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                return NotFound("Record you are searching for does not exist");
            }
        }

        // POST api/dish
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Dish dish)
        {
            try
            {
                await _repository.Insert(dish);
                return Ok("Record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                return BadRequest("Error: Record was not inserted\n");
            }
        }

            // PUT api/dish/5
            [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Dish dish)
        {
            
            if (id != dish.Dish_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                var response = await _repository.GetById(id);

                if (response == null)
                {
                    return NotFound("Record was not found\n");
                }
                else
                {
                    await _repository.ModifyById(dish);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                return BadRequest("Error: Record could not be updated\n");
            } 
        }

        // DELETE api/dish/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _repository.GetById(id);
                await _repository.DeleteById(id);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, id));
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                return BadRequest("Error: Record could not be deleted\n");
            }
        }
    }
        
}

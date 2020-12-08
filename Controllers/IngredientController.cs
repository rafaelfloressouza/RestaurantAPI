using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class IngredientController : Controller
    {
        private readonly IngredientRepository _repository;
        
        public IngredientController(IngredientRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/ingredient
        [HttpGet]
        public async Task<List<Ingredient>> Get()
        {
            // Getting all records from the Ingredient table
            return await _repository.GetAll();
        }

        // GET api/ingredient/potato
        [HttpGet("{ing_name}")]
        public async Task<ActionResult<Ingredient>> Get(string ing_name)
        {
            // Making sure that ingredient name is title case
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            ing_name = textInfo.ToTitleCase(ing_name.ToLower());

            try
            {
                // Searching for record in the database
                var response = await _repository.GetByName(ing_name);
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
                return NotFound("Ingredient record you are searching for does not exist or URL is wrong\n");
            }
        }

        // POST api/ingredient
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Ingredient ingredient)
        {
            // Making sure that ingredient name is title case
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            ingredient.Name = textInfo.ToTitleCase(ingredient.Name.ToLower());

            try
            {
                // Inserting record in the Dish table
                await _repository.Insert(ingredient);
                return Ok("Ingredient record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Ingredient record was not inserted\n");
            }
        }

        // PUT api/ingredient/potato
        [HttpPut("{ing_name}")]
        public async Task<ActionResult> Put(string ing_name, [FromBody] Ingredient ingredient)
        {
            // Making sure that ingredient name is title case
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            ing_name = textInfo.ToTitleCase(ing_name.ToLower());
            ingredient.Name = textInfo.ToTitleCase(ingredient.Name.ToLower());

            // If ing_name in body does not match ing_name in URL
            if (!ing_name.Equals(ingredient.Name))
            {
                return BadRequest("Ingredient name in URL has to match the ingredient name of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetByName(ing_name);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Ingredient record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyByName(ingredient);
                    string format = "Ingredient record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, ing_name));
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
                return BadRequest("Error: Ingredient record could not be updated\n");
            }
        }

        // DELETE api/ingredient/potato
        [HttpDelete("{ing_name}")]
        public async Task<ActionResult> Delete(string ing_name)
        {
            // Making sure that ingredient name is title case
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            ing_name = textInfo.ToTitleCase(ing_name.ToLower());

            try
            {
                // Searching for record in the Ingredient table
                var response = await _repository.GetByName(ing_name);

                // Deleting record from Ingredient table
                await _repository.DeleteByName(ing_name);
                string format = "Ingredient record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, ing_name));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Ingredient record could not be deleted\n");
            }
        }
    }
}

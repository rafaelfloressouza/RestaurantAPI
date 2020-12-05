using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;
using System.Globalization;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {

        private readonly MenuRepository _repository;

        public MenuController(MenuRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/menu
        [HttpGet]
        public async Task<List<Menu>> Get()
        {
            // Getting all records from the Menu table
            return await _repository.GetAll();
        }

        // GET api/Menu/vegetarian
        [HttpGet("{type}")]
        public async Task<ActionResult<Menu>> Get(string type)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            type = textInfo.ToTitleCase(type.ToLower());

            try
            {
                // Searching for record in the database
                var response = await _repository.GetByType(type);
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

        // POST api/menu
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Menu menu)
        {
            try
            {
                // Inserting record in the Manu table
                await _repository.Insert(menu);
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

        // PUT api/menu/vegetarian
        [HttpPut("{type}")]
        public async Task<ActionResult> Put(string type, [FromBody] Menu menu)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            type = textInfo.ToTitleCase(type.ToLower());


            // If id in body does not match id in URL
            if (!type.Equals(menu.Type))
            {
                return BadRequest("type in URL has to match the type of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetByType(type);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyByType(menu);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, type));
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

        // DELETE api/menu/vegetarian
        [HttpDelete("{type}")]
        public async Task<ActionResult> Delete(string type)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            type = textInfo.ToTitleCase(type.ToLower());

            try
            {
                // Searching for record inn the Menu table
                var response = await _repository.GetByType(type);

                // Deleting record from Menu
                await _repository.DeleteByType(type);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, type));
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

        //api/menu/getDishes/normal
        [Route("getDishes/{type}")]
        [HttpGet]
        public async Task<List<Dish>> getDishes(string type)
        {
            // Getting all dishes for a specific menu 
            return await _repository.getDishes(type);
        }
    }
}

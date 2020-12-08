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
    public class Ingredient_SupplierController : Controller
    {

        private readonly Ingredient_SupplierRepository _repository;
        private readonly IngredientRepository _ingredientRepository;
        private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        
        public Ingredient_SupplierController(Ingredient_SupplierRepository repository, IngredientRepository ingredientRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _ingredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository));
        }

        // GET: api/ingredient_supplier
        [HttpGet]
        public async Task<List<Ingredient_Supplier>> Get()
        {
            // Getting all records from the Ingredient_Supplier table
            return await _repository.GetAll();
        }

        // GET api/ingredient_supplier/potato/michael's
        [HttpGet("{ing_name}/{supplier}")]
        public async Task<ActionResult<Ingredient_Supplier>> Get(string ing_name, string supplier)
        {
            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(supplier, ing_name);
                return response;
                
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch (ArgumentNullException)
            {
                return NotFound("ERROR: Ingredient_Supplier record you are searching was not found. Make sure the ingredient name and supplier are correct");
            }
            catch
            {
                // Unknown error
                return NotFound("Record you are searching for does not exist");
            }
        }

        // POST api/ingredient_supplier
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Ingredient_Supplier ing_sup)
        {
            ing_sup.Ing_Name = textInfo.ToTitleCase(ing_sup.Ing_Name.ToLower());

            try
            {
                // Inserting record in the Ingredient_Supplier table
                await _repository.Insert(ing_sup);
                return Ok("Ingredient_Supplier record inserted successfully\n");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());

            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Ingredient_Supplier record was not inserted\n");
            }
        }

        // PUT api/ingredient_supplier
        [HttpPut]
        public ActionResult Put()
        {
            // We cannot modify entries in the Dish_Ingredient table. It has to be done directly through deletes and posts
            return BadRequest("ERROR: You cannot modify entries in the Ingredient_Supplier table. Try using POST and DELETE instead.\n");
        }

        // DELETE api/ingredient_supplier/potato/michale's
        [HttpDelete("{ing_name}/{supplier}")]
        public async Task<ActionResult> Delete(string ing_name, string supplier)
        {
            try
            {
                // Searching for record in the Ingredient_Supplier table
                var response = await _repository.GetById(supplier, ing_name);

                string format1 = "Ingredient_Supplier record in the Ingredient Supplier table with key=({0},{1}) deleted succesfully\n";
                string format2 = "Record in the Ingredient table with key={0} deleted because ingredients need suppliers and the only supplier of the ingredient was deleted\n";
 
                // Getting number of supplier for that ingredient
                if (await _repository.getNumberOfSuppliers(ing_name) == 1)
                {
                    // Deleting record from Ingredient_Supplier table and Ingredient Table
                    // Due to foreign key constrains we can simply delete the ingredient from the Ingredient table
                    await _ingredientRepository.DeleteByName(ing_name);
                    return Ok(string.Format(format2, ing_name));
                }
                else
                {
                    // Deleting record from Ingredient_Supplier table
                    await _repository.DeleteById(supplier, ing_name);
                    return Ok(string.Format(format1, ing_name, supplier));
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
                return BadRequest("Error: Ingredient_Supplier record could not be deleted\n");
            }
        }

        //api/ingredient_supplier/getNumSuppliers/potato
        [Route("getNumSuppliers/{ing_name}")]
        [HttpGet]
        public async Task<ActionResult> getNumSuppliers(string ing_name)
        {
            try
            {
                // There is no error and we are able to retrieve the number of suppliers
                string format = "The number of suppliers for {0} is/are {1}\n";
                return Ok(string.Format(format, ing_name, await _repository.getNumberOfSuppliers(ing_name)));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Some unknown exception
                return BadRequest("ERROR: Number of suppliers for that record could not be retrieved");
            }
        }
    }
}

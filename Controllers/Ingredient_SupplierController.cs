using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using RestaurantAPI.Context;    

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Ingredient_SupplierController : Controller
    {
        private readonly AppDBContext context;

        public Ingredient_SupplierController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/ingredient_supplier
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Ingredient_Supplier.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/ingredient_supplier/ing_name/supplier
        [HttpGet("{Ing_Name}/{Supplier}", Name ="GetIngredientSupplier")]
        public ActionResult Get(string Ing_Name, string Supplier)
        {
            try
            {
                var ingredient_supplier = context.Ingredient_Supplier.FirstOrDefault(f => f.Ing_Name.ToLower().Equals(Ing_Name.ToLower()) && f.Supplier.ToLower().Equals(Supplier.ToLower()));
                return Ok(ingredient_supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/ingredient_supplier
        [HttpPost]
        public ActionResult Post([FromBody] Ingredient_Supplier ingredient_supplier)
        {
            try
            {
                context.Ingredient_Supplier.Add(ingredient_supplier);
                context.SaveChanges();
                return CreatedAtRoute("GetIngredientSupplier", new { ING_NAME = ingredient_supplier.Ing_Name, SUPPLIER = ingredient_supplier.Supplier }, ingredient_supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/ingredient_supplier
        [HttpPut]
        public ActionResult Put()
        {

            return BadRequest("Elements in the Ingredient_Supplier table cannot be changed");
        }

        // DELETE api/ingredient_supplier/5
        [HttpDelete("{Ing_Name}/{Supplier}")]
        public ActionResult Delete(string Ing_Name, string Supplier)
        {
            // Attempting to delete the element from the table in the DB
            try
            {
                var ingredient_supplier = context.Ingredient_Supplier.FirstOrDefault(f => f.Ing_Name.ToLower().Equals(Ing_Name.ToLower()) && f.Supplier.ToLower().Equals(Supplier.ToLower()));
                if (ingredient_supplier != null)
                {
                    context.Ingredient_Supplier.Remove(ingredient_supplier);
                    context.SaveChanges();
                    return Ok(new { Ing_Name, Supplier});
                }
                else
                {
                    return BadRequest("Error while trying to delete the element\n");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

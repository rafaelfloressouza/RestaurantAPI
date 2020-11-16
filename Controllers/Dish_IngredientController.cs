using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Dish_IngredientController : Controller
    {
        private readonly AppDBContext context;

        public Dish_IngredientController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/dish_ingredient
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Dish_Ingredient.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/dish_ingredient/5/6
        [HttpGet("{Dish_ID}/{Ing_Name}", Name ="GetDishIngredient")]
        public ActionResult Get(int Dish_ID, string Ing_Name)
        {
            try
            {
                var dish_ingredient = context.Dish_Ingredient.FirstOrDefault(f => f.Dish_ID == Dish_ID && f.Ing_Name.Equals(Ing_Name));
                return Ok(dish_ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/dish_ingredient
        [HttpPost]
        public ActionResult Post([FromBody] Dish_Ingredient dish_ingredient)
        {
            try
            {
                context.Dish_Ingredient.Add(dish_ingredient);
                context.SaveChanges();
                return CreatedAtRoute("GetDishIngredient", new { DISH_ID = dish_ingredient.Dish_ID, ING_NAME = dish_ingredient.Ing_Name}, dish_ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/dish_ingredient
        [HttpPut]
        public ActionResult Put()
        {
            return BadRequest("Elements in the Dish_Ingredient table cannot be changed.");
        }

        // DELETE api/dish_ingredient/5/6
        [HttpDelete("{Dish_ID}/{Ing_Name}")]
        public ActionResult Delete(int Dish_ID, string Ing_Name)
        {
            try
            {
                var dish_ingredient = context.Dish_Ingredient.FirstOrDefault(f => f.Dish_ID == Dish_ID && f.Ing_Name.Equals(Ing_Name));
                if (dish_ingredient != null)
                {
                    context.Dish_Ingredient.Remove(dish_ingredient);
                    context.SaveChanges();
                    return Ok(new { Dish_ID, Ing_Name});
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

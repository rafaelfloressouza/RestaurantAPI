using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RAPI2.Context;
using RAPI2.Models;
using Microsoft.EntityFrameworkCore;

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class DishController : Controller
    {

        private readonly AppDBContext context;

        public DishController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/dish
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Dish.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/dish/5
        [HttpGet("{id}", Name= "GetDish")]
        public ActionResult Get(int id)
        {
            try
            {
                var dish = context.Dish.FirstOrDefault(f => f.Dish_ID == id);
                return Ok(dish);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/dish
        [HttpPost]
        public ActionResult Post([FromBody] Dish dish)
        {
            try
            {
                context.Dish.Add(dish);
                context.SaveChanges();
                return CreatedAtRoute("GetDish", new {ID = dish.Dish_ID }, dish);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/dish/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Dish dish)
        {
            try
            {
                if (dish.Dish_ID == id)
                {
                    context.Entry(dish).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetDish", new { ID = dish.Dish_ID }, dish);
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

        // DELETE api/dish/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var dish = context.Dish.FirstOrDefault(f => f.Dish_ID == id);
                if (dish != null)
                {
                    context.Dish.Remove(dish);
                    context.SaveChanges();
                    return Ok(id);
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

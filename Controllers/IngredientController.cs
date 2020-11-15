using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RAPI2.Context;
using RAPI2.Models;
using Microsoft.EntityFrameworkCore;

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class IngredientController : Controller
    {
        private readonly AppDBContext context;

        public IngredientController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/ingredient
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Ingredient.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/ingredient/name
        [HttpGet("{Name}", Name ="GetIngredient")]
        public ActionResult Get(string Name)
        {
            try
            {
                var ingredient = context.Ingredient.FirstOrDefault(f => f.Name.Equals(Name));
                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/ingredient
        [HttpPost]
        public ActionResult Post([FromBody] Ingredient ingredient)
        {
            try
            {
                context.Ingredient.Add(ingredient);
                context.SaveChanges();
                return CreatedAtRoute("GetIngredient", new { NAME = ingredient.Name }, ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/ingredient/name
        [HttpPut("{Name}")]
        public ActionResult Put(string Name, [FromBody] Ingredient ingredient)
        {
            try
            {
                if (ingredient.Name.Equals(Name))
                {
                    context.Entry(ingredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetIngredient", new { NAME = ingredient.Name }, ingredient);
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

        // DELETE api/ingredient/5
        [HttpDelete("{Name}")]
        public ActionResult Delete(string Name)
        {
            try
            {
                var ingredient = context.Ingredient.FirstOrDefault(f => f.Name.Equals(Name));
                if (ingredient != null)
                {
                    context.Ingredient.Remove(ingredient);
                    context.SaveChanges();
                    return Ok(Name);
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

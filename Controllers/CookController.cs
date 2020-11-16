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
    public class CookController : Controller
    {
        private readonly AppDBContext context;

        public CookController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/cook
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Cook.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/cook/5
        [HttpGet("{id}", Name ="GetCook")]
        public ActionResult Get(int id)
        {
            try
            {
                var cook = context.Cook.FirstOrDefault(f => f.User_ID == id);
                return Ok(cook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/cook
        [HttpPost]
        public ActionResult Post([FromBody] Cook cook)
        {
            try
            {
                context.Cook.Add(cook);
                context.SaveChanges();
                return CreatedAtRoute("GetCook", new { ID = cook.User_ID }, cook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/cook/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Cook cook)
        {
            try
            {
                if (cook.User_ID == id)
                {
                    context.Entry(cook).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetCook", new { ID = cook.User_ID }, cook);
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

        // DELETE api/cook/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cook = context.Cook.FirstOrDefault(f => f.User_ID == id);
                if (cook != null)
                {
                    context.Cook.Remove(cook);
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

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
    public class MenuController : Controller
    {
        private readonly AppDBContext context;

        public MenuController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/menu
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Menu.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/menu/Vegeterian 
        [HttpGet("{Type}",Name ="GetMenu")]
        public ActionResult Get(string Type)
        {
            try
            {
                var menu = context.Menu.FirstOrDefault(f => f.Type.Equals(Type));
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/menu
        [HttpPost]
        public ActionResult Post([FromBody] Menu menu)
        {
            try
            {
                context.Menu.Add(menu);
                context.SaveChanges();
                return CreatedAtRoute("GetMenu", new { TYPE = menu.Type }, menu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/menu/Vegeterian
        [HttpPut("{Type}")]
        public ActionResult Put(string Type, [FromBody] Menu menu)
        {
            try
            {
                if (menu.Type.Equals(Type))
                {
                    context.Entry(menu).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetMenu", new { TYPE = menu.Type }, menu);
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

        // DELETE api/menu/5
        [HttpDelete("{Type}")]
        public ActionResult Delete(string Type)
        {
            try
            {
                var menu = context.Menu.FirstOrDefault(f => f.Type.Equals(Type));
                if (menu != null)
                {
                    context.Menu.Remove(menu);
                    context.SaveChanges();
                    return Ok(Type);
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

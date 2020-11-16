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
    public class WaiterController : Controller
    {
        private readonly AppDBContext context;

        public WaiterController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/waiter
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Waiter.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/waiter/5
        [HttpGet("{id}", Name ="GetWaiter")]
        public ActionResult Get(int id)
        {
            try
            {
                var waiter = context.Waiter.FirstOrDefault(f => f.User_ID == id);
                return Ok(waiter);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/waiter
        [HttpPost]
        public ActionResult Post([FromBody] Waiter waiter)
        {
            try
            {
                context.Waiter.Add(waiter);
                context.SaveChanges();
                return CreatedAtRoute("GetWaiter", new { ID = waiter.User_ID }, waiter);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/waiter/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Waiter waiter)
        {
            try
            {
                if (waiter.User_ID == id)
                {
                    context.Entry(waiter).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetWaiter", new { ID = waiter.User_ID }, waiter);
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

        // DELETE api/waiter/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var waiter = context.Waiter.FirstOrDefault(f => f.User_ID == id);
                if (waiter != null)
                {
                    context.Waiter.Remove(waiter);
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

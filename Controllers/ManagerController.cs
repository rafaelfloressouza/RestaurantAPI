using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class ManagerController : Controller
    {
        private readonly AppDBContext context;

        public ManagerController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/manager
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Manager.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/manager/5
        [HttpGet("{id}", Name="GetManager")]
        public ActionResult Get(int id)
        {
            try
            {
                var manager = context.Manager.FirstOrDefault(f => f.User_ID == id);
                return Ok(manager);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/manager
        [HttpPost]
        public ActionResult Post([FromBody] Manager manager)
        {
            try
            {
                context.Manager.Add(manager);
                context.SaveChanges();
                return CreatedAtRoute("GetManager", new { ID = manager.User_ID }, manager);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/manager/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Manager manager)
        {
            try
            {
                if (manager.User_ID == id)
                {
                    context.Entry(manager).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetManager", new { ID = manager.User_ID }, manager);
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

        // DELETE api/manager/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var manager = context.Manager.FirstOrDefault(f => f.User_ID == id);
                if (manager != null)
                {
                    context.Manager.Remove(manager);
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

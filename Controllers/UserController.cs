using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAPI2.Context;
using RAPI2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AppDBContext context;

        public UserController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/user
        [HttpGet]
        public ActionResult Get()
        {
            try { 
                return Ok(context.User.ToList());
            }catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET api/user/5
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult Get(int id)
        {
            try
            {
                var user = context.User.FirstOrDefault(f => f.ID == id);
                return Ok(user);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/user
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            try
            {
                context.User.Add(user);
                context.SaveChanges();
                return CreatedAtRoute("GetUser", new { ID = user.ID }, user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User user)
        {
            try
            {
                if (user.ID == id)
                {
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetUser", new { ID = user.ID }, user );
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

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = context.User.FirstOrDefault(f => f.ID == id);
                if(user != null)
                {
                    context.User.Remove(user);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class CookController : Controller
    {
        private readonly CookRepository _repository;

        public CookController(CookRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/cook
        [HttpGet]
        public async Task<List<Cook>> Get()
        {
            return await _repository.GetAll();
        }


        // GET api/cook/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cook>> Get(int id)
        {
            try
            {
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                return NotFound("Record you are searching for does not exist");
            }
        }

        /*
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
        */
    }
}

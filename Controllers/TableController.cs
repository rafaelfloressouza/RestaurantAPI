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
    public class TableController : Controller
    {
        private readonly AppDBContext context;

        public TableController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/table
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Table.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/table/5
        [HttpGet("{id}", Name ="GetTable")]
        public ActionResult Get(int id)
        {
            try
            {
                var table = context.Table.FirstOrDefault(f => f.TableNo == id);
                return Ok(table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/table
        [HttpPost]
        public ActionResult Post([FromBody] Table table)
        {
            try
            {
                context.Table.Add(table);
                context.SaveChanges();
                return CreatedAtRoute("GetTable", new { ID = table.TableNo }, table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/table/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Table table)
        {
            try
            {
                if (table.TableNo == id)
                {
                    context.Entry(table).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetTable", new { ID = table.TableNo }, table);
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

        // DELETE api/table/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var table = context.Table.FirstOrDefault(f => f.TableNo == id);
                if (table != null)
                {
                    context.Table.Remove(table);
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

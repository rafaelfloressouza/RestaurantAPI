using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RAPI2.Context;
using RAPI2.Models;
using Microsoft.EntityFrameworkCore;

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly AppDBContext context;

        public OrderController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/order
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Order.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/order/5
        [HttpGet("{id}", Name ="GetOrder")]
        public ActionResult Get(int id)
        {
            try
            {
                var order = context.Order.FirstOrDefault(f => f.Order_ID == id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/order
        [HttpPost]
        public ActionResult Post([FromBody] Order order)
        {
            try
            {
                context.Order.Add(order);
                context.SaveChanges();
                return CreatedAtRoute("GetOrder", new { ID = order.Order_ID }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/order/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Order order)
        {
            try
            {
                if (order.Order_ID == id)
                {
                    context.Entry(order).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetOrder", new { ID = order.Order_ID }, order);
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

        // DELETE api/order/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var order = context.Order.FirstOrDefault(f => f.Order_ID == id);
                if (order != null)
                {
                    context.Order.Remove(order);
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

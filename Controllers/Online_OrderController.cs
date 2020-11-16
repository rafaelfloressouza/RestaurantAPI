using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using RestaurantAPI.Context;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Online_OrderController : Controller
    {
        private readonly AppDBContext context;

        public Online_OrderController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/online_order
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Online_Order.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/online_order/5
        [HttpGet("{id}", Name ="GetOnlineOrder")]
        public ActionResult Get(int id)
        {
            try
            {
                var online_order = context.Online_Order.FirstOrDefault(f => f.Order_ID == id);
                return Ok(online_order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/online_order
        [HttpPost]
        public ActionResult Post([FromBody] Online_Order online_order)
        {
            try
            {
                context.Online_Order.Add(online_order);
                context.SaveChanges();
                return CreatedAtRoute("GetOnlineOrder", new { ID = online_order.Order_ID }, online_order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/online_order/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Online_Order online_order)
        {
            try
            {
                if (online_order.Order_ID == id)
                {
                    context.Entry(online_order).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetOnlineOrder", new { ID = online_order.Order_ID}, online_order);
                }
                else
                {
                    return BadRequest("The id argument must be equal to the id of the element you are trying to modify\n");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/online_order/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var online_order = context.Online_Order.FirstOrDefault(f => f.Order_ID == id);
                if (online_order != null)
                {
                    // Searching for Order
                    var order = context.Order.FirstOrDefault(f => f.Order_ID == id);
                    context.Order.Remove(order);
                    context.SaveChanges();
                    return Ok(new { online_order, order});
                }
                else
                {
                    return BadRequest("The element you are trying to delete does not exist\n");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

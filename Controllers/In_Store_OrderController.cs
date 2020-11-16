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
    public class In_Store_OrderController : Controller
    {

        private readonly AppDBContext context;

        public In_Store_OrderController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/in_store_order
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.In_Store_Order.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/in_store_order/5
        [HttpGet("{id}", Name ="GetInStoreOrder")]
        public ActionResult Get(int id)
        {
            try
            {
                var in_store_order = context.In_Store_Order.FirstOrDefault(f => f.Order_ID == id);
                return Ok(in_store_order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/in_store_order
        [HttpPost]
        public ActionResult Post([FromBody] In_Store_Order in_store_order)
        {
            try
            {
                context.In_Store_Order.Add(in_store_order);
                context.SaveChanges();
                return CreatedAtRoute("GetInStoreOrder", new { ID = in_store_order.Order_ID }, in_store_order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/in_store_order/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] In_Store_Order in_store_order)
        {
            try
            {
                if (in_store_order.Order_ID == id)
                {
                    context.Entry(in_store_order).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetInStoreOrder", new { ID = in_store_order.Order_ID }, in_store_order);
                }
                else
                {
                    return BadRequest("ERROR: Make sure the pased id matches the id of the element you want to modify\n");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/in_store_order/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var in_store_order = context.In_Store_Order.FirstOrDefault(f => f.Order_ID == id);
                if (in_store_order != null)
                {
                    // Searching for Order
                    var order = context.Order.FirstOrDefault(f => f.Order_ID == id);
                    context.Order.Remove(order);
                    context.SaveChanges();
                    return Ok(new {in_store_order, order});
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

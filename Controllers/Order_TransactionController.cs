using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Order_TransactionController : Controller
    {
        private readonly AppDBContext context;

        public Order_TransactionController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/order_transaction
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Order_Transaction.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/order_transaction/5/6
        [HttpGet("{Order_ID}/{Transaction_ID}", Name ="GetOrderTransaction")]
        public ActionResult Get(int Order_ID, int Transaction_ID)
        {
            try
            {
                var order_transaction = context.Order_Transaction.FirstOrDefault(f => f.Order_ID == Order_ID && f.Transaction_ID == Transaction_ID);
                return Ok(order_transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/order_transaction
        [HttpPost]
        public ActionResult Post([FromBody] Order_Transaction order_transaction)
        {
            try
            {
                context.Order_Transaction.Add(order_transaction);
                context.SaveChanges();
                return CreatedAtRoute("GetOrderTransaction", new { Order_ID = order_transaction.Order_ID, Transaction_ID = order_transaction.Transaction_ID }, order_transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/order_transaction
        [HttpPut]
        public ActionResult Put()
        {
            return BadRequest("Elements in the Order_Transaction table cannot be changed");
        }

        // DELETE api/order_transaction/5/6
        [HttpDelete("{Order_ID}/{Transaction_ID}")]
        public ActionResult Delete(int Order_ID, int Transaction_ID)
        {
            try
            {
                var order_transaction = context.Order_Transaction.FirstOrDefault(f => f.Order_ID == Order_ID && f.Transaction_ID == Transaction_ID);
                if (order_transaction != null)
                {
                    var num_of_orders_in_transaction = context.Order_Transaction.ToList().Count(f => f.Transaction_ID == Transaction_ID);

                    if (num_of_orders_in_transaction <= 1)
                    {
                        var transaction = context.Transaction.FirstOrDefault(f => f.Transaction_ID == Transaction_ID);
                        context.Transaction.Remove(transaction);
                        context.SaveChanges();
                        return Ok(new { Order_ID, Transaction_ID, transaction });
                    }
                    else
                    {
                        context.Order_Transaction.Remove(order_transaction);
                        context.SaveChanges();
                        return Ok(new { Order_ID, Transaction_ID});
                    }
                }
                else
                {
                    return BadRequest("Element that you are trying to delete does not exist\n");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

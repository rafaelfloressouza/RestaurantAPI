using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Order_TableController : Controller
    {
        private readonly AppDBContext context;

        public Order_TableController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/order_table
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Order_Table.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/order_table/5/6
        [HttpGet("{Order_ID}/{TableNo}", Name="GetOrderTable")]
        public ActionResult Get(int Order_ID, int TableNo)
        {
            try
            {
                var order_table = context.Order_Table.FirstOrDefault(f => f.Order_ID == Order_ID && f.TableNo == TableNo);
                return Ok(order_table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/order_table
        [HttpPost]
        public ActionResult Post([FromBody] Order_Table order_table)
        {
            try
            {
                context.Order_Table.Add(order_table);
                context.SaveChanges();
                return CreatedAtRoute("GetOrderTable", new { Order_ID = order_table.Order_ID, TableNo = order_table.TableNo }, order_table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/order_table
        [HttpPut]
        public ActionResult Put()
        {
            return BadRequest("Elements in the Order_Table table cannot be changed");
        }

        // DELETE api/order_table/5/6
        [HttpDelete("{Order_ID}/{TableNo}")]
        public ActionResult Delete(int Order_ID, int TableNo)
        {
            try
            {
                var order_table = context.Order_Table.FirstOrDefault(f => f.Order_ID == Order_ID && f.TableNo == TableNo);
                if (order_table != null)
                {
                    var num_of_orders_of_table = context.Order_Table.ToList().Count(f => f.Order_ID == Order_ID);

                    if (num_of_orders_of_table <= 1)
                    {
                        var order = context.Order.FirstOrDefault(f => f.Order_ID == Order_ID);
                        context.Order.Remove(order);
                        context.SaveChanges();
                        return Ok(new { Order_ID, TableNo, order });
                    }
                    else
                    {
                        context.Order_Table.Remove(order_table);
                        context.SaveChanges();
                        return Ok(new { Order_ID, TableNo });
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

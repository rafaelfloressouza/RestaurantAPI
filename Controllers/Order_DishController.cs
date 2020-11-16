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
    public class Order_DishController : Controller
    {
        private readonly AppDBContext context;

        public Order_DishController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/order_dish
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Order_Dish.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/order_dish/5/6
        [HttpGet("{Order_ID}/{Dish_ID}", Name ="GetOrderDish")]
        public ActionResult Get(int Order_ID, int Dish_ID)
        {
            try
            {
                var order_dish = context.Order_Dish.FirstOrDefault(f => f.Order_ID == Order_ID && f.Dish_ID == Dish_ID);
                return Ok(order_dish);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/order_dish
        [HttpPost]
        public ActionResult Post([FromBody] Order_Dish order_dish)
        {
            try
            {
                context.Order_Dish.Add(order_dish);
                context.SaveChanges();
                return CreatedAtRoute("GetOrderDish", new { Order_ID = order_dish.Order_ID, Dish_ID = order_dish.Dish_ID}, order_dish);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/order_dish
        [HttpPut]
        public ActionResult Put()
        {
            return BadRequest("Elements in the Order_Dish table cannot be changed");
        }

        // DELETE api/order_dish/5/6
        [HttpDelete("{Order_ID}/{Dish_ID}")]
        public ActionResult Delete(int Order_ID, int Dish_ID)
        {
            try
            {
                var order_dish = context.Order_Dish.FirstOrDefault(f => f.Order_ID == Order_ID && f.Dish_ID == Dish_ID);
                if (order_dish != null)
                {
                    var num_of_dishes_in_order = context.Order_Dish.ToList().Count(f => f.Order_ID == Order_ID);

                    if (num_of_dishes_in_order <= 1)
                    {
                        var order = context.Order.FirstOrDefault(f => f.Order_ID == Order_ID);
                        context.Order.Remove(order);
                        context.SaveChanges();
                        return Ok(new { Order_ID, Dish_ID, order });
                    }
                    else
                    {
                        context.Order_Dish.Remove(order_dish);
                        context.SaveChanges();
                        return Ok(new { Order_ID, Dish_ID });
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

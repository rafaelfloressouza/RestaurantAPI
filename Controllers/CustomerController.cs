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
    public class CustomerController : Controller
    {

        private readonly AppDBContext context;

        public CustomerController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/customer
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Customer.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/customer/5
        [HttpGet("{id}", Name="GetCustomer")]
        public ActionResult Get(int id)
        {
            try
            {
                var customer = context.Customer.FirstOrDefault(f => f.User_ID == id);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/customer
        [HttpPost]
        public ActionResult Post([FromBody] Customer customer)
        {
            try
            {
                context.Customer.Add(customer);
                context.SaveChanges();
                return CreatedAtRoute("GetCustomer", new { ID = customer.User_ID }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/customer/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Customer customer)
        {
            try
            {
                if (customer.User_ID == id)
                {
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetCustomer", new { ID = customer.User_ID }, customer);
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

        // DELETE api/customer/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var customer = context.Customer.FirstOrDefault(f => f.User_ID == id);
                if (customer != null)
                {
                    context.Customer.Remove(customer);
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

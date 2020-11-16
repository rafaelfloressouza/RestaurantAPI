using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;
using System;
using System.Linq;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class Customer_TransactionController : Controller
    {
        private readonly AppDBContext context;

        public Customer_TransactionController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/customer_transaction
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Customer_Transaction.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/customer_transaction/5/6
        [HttpGet("{User_ID}/{Transaction_ID}", Name="GetCustomerTransaction")]
        public ActionResult Get(int User_Id, int Transaction_ID)
        {
            try
            {
                var customer_transaction = context.Customer_Transaction.FirstOrDefault(f => f.User_ID == User_Id && f.Transaction_ID == Transaction_ID);
                return Ok(customer_transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/customer_transaction
        [HttpPost]
        public ActionResult Post([FromBody] Customer_Transaction customer_transaction)
        {
            try
            {
                context.Customer_Transaction.Add(customer_transaction);
                context.SaveChanges();
                return CreatedAtRoute("GetCustomerTransaction", new { USER_ID = customer_transaction.User_ID, TRANSACTION_ID = customer_transaction.Transaction_ID }, customer_transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT  api/customer_transaction
        [HttpPut]
        public ActionResult Put()
        {
            return BadRequest("Elements in the Customer_Transaction table cannot be changed");
        }

        // DELETE api/customer_transaction/5/6
        [HttpDelete("{User_ID}/{Transaction_ID}")]
        public ActionResult Delete(int User_ID, int Transaction_ID)
        {
            try
            {
                var customer_transaction = context.Customer_Transaction.FirstOrDefault(f => f.User_ID == User_ID && f.Transaction_ID == Transaction_ID);
                if (customer_transaction != null)
                {
                    
                    // Searching for transaction to delete (so it is not dangling)
                    var transaction = context.Transaction.FirstOrDefault(f => f.Transaction_ID == Transaction_ID);

                    context.Customer_Transaction.Remove(customer_transaction);
                    context.Transaction.Remove(transaction);
                    context.SaveChanges();
                    return Ok(new {User_ID, Transaction_ID, transaction});
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

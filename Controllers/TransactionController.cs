using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Context;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly AppDBContext context;

        public TransactionController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/transaction
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Transaction.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/transaction/5
        [HttpGet("{id}", Name = "GetTransaction")]
        public ActionResult Get(int id)
        {
            try
            {
                var transaction = context.Transaction.FirstOrDefault(f => f.Transaction_ID == id);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/transaction
        [HttpPost]
        public ActionResult Post([FromBody] Transaction transaction)
        {
            try
            {
                context.Transaction.Add(transaction);
                context.SaveChanges();
                return CreatedAtRoute("GetTransaction", new { ID = transaction.Transaction_ID }, transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/transaction/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Transaction transaction)
        {
            try
            {
                if (transaction.Transaction_ID== id)
                {
                    context.Entry(transaction).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetTransaction", new { ID = transaction.Transaction_ID }, transaction);
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

        // DELETE api/transaction/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var transaction = context.Transaction.FirstOrDefault(f => f.Transaction_ID == id);
                if (transaction != null)
                {
                    context.Transaction.Remove(transaction);
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

using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Context;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly AppDBContext context;

        public ReviewController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/review
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Review.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/review/5
        [HttpGet("{id}", Name ="GetReview")]
        public ActionResult Get(int id)
        {
            try
            {
                var review = context.Review.FirstOrDefault(f => f.Review_ID == id);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Review review)
        {
            try
            {
                context.Review.Add(review);
                context.SaveChanges();
                return CreatedAtRoute("GetReview", new { ID = review.Review_ID }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/review/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Review review)
        {
            try
            {
                if (review.Review_ID == id)
                {
                    context.Entry(review).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetReview", new { ID = review.Review_ID }, review);
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

        // DELETE api/review/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var review = context.Review.FirstOrDefault(f => f.Review_ID == id);
                if (review != null)
                {
                    context.Review.Remove(review);
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

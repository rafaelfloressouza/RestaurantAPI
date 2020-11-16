using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly AppDBContext context;

        public EmployeeController(AppDBContext context)
        {
            this.context = context;
        }

        // GET: api/employee
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.Employee.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/employee/5
        [HttpGet("{id}", Name= "GetEmployee")]
        public ActionResult Get(int id)
        {
            try
            {
                var employee = context.Employee.FirstOrDefault(f => f.User_ID == id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/employee
        [HttpPost]
        public ActionResult Post([FromBody] Employee employee)
        {
            try
            {
                context.Employee.Add(employee);
                context.SaveChanges();
                return CreatedAtRoute("GetEmployee", new { ID = employee.User_ID}, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/employee/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Employee employee)
        {
            try
            {
                if (employee.User_ID == id)
                {
                    context.Entry(employee).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetUser", new { ID = employee.User_ID }, employee);
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

        // DELETE api/employee/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var employee = context.Employee.FirstOrDefault(f => f.User_ID == id);
                if (employee != null)
                {
                    context.Employee.Remove(employee);
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

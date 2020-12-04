using System;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace RAPI2.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _repository;
        private readonly CustomerRepository _customerRepository;
        private readonly CookRepository _cookRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly ManagerRepository _managerRepository;
        private readonly WaiterRepository _waiterRepository;

        public UserController(UserRepository repository, CustomerRepository customerRepository, CookRepository cookRepository, EmployeeRepository employeeRepository, ManagerRepository managerRepository, WaiterRepository waiterRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _cookRepository = cookRepository ?? throw new ArgumentException(nameof(cookRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentException(nameof(employeeRepository));
            _managerRepository = managerRepository ?? throw new ArgumentNullException(nameof(managerRepository));
            _waiterRepository = waiterRepository ?? throw new ArgumentNullException(nameof(waiterRepository));
        }

        // GET: api/user
        [HttpGet]
        public async Task<List<User>> Get()
        {
            // Getting all records from the User tables
            return await _repository.GetAll();
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                // Searching for record in the User table
                var response = await _repository.GetById(id);
                return response;
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return NotFound("Record you are searching for does not exist");
            }
        }

        // POST api/user/employee/cook
        // POST api/user/customer/
        [HttpPost("{category}/{subcategory?}")]
        public async Task<ActionResult> Post([FromBody] User user, string category, string subcategory = "None")
        {
            // Useful string variables
            category = category.ToLower();
            subcategory = subcategory.ToLower();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            // If the category does not match any existing category
            if (!(category.Equals("employee") || category.Equals("customer")))
            {
                return BadRequest("Wrong category! Try Employee or Customer\n");
            }

            // If the subcategory does not match any existing subcategory
            if (category.Equals("employee") && !subcategory.Equals("none") && !(subcategory.Equals("cook") || subcategory.Equals("manager") || subcategory.Equals("waiter")))
            {
                return BadRequest("Wrong subcategory! Try Cook, Manager, or Waiter\n");
            }

            // A valid category was provided
            try
            {
                // Inserting new record into the User table
                await _repository.Insert(user);
                int cur_user_id = await _repository.getLastInsertedID();

                if (subcategory.Equals("none") && category.Equals("customer"))
                {
                    // There is no subcategory, we insert the new entry into the User and Customer tables
                    await _customerRepository.Insert(new Customer { User_ID = cur_user_id, TableNo = null });
                }
                else if (!subcategory.Equals("none") && category.Equals("employee"))// There is a subcategory, we insert the new entry into Employee (with default values), and the respective subcategory
                {
                    // Inserting new record into the Employee table
                    await _employeeRepository.Insert(new Employee { User_ID = cur_user_id, Start_Date = DateTime.Today, Job_Title = textInfo.ToTitleCase(subcategory.ToLower()).ToString(), Salary = (decimal)4500.00, mgr_ID = null });

                    // Inserting new record into the respective subcategory of employee
                    if (subcategory.Equals("cook"))
                    {
                        // Inserting new record into Cook table (with default values)
                        await _cookRepository.Insert(new Cook { User_ID = cur_user_id, Specialty = "", Type = "Full" });
                    }
                    else if (subcategory.Equals("waiter"))
                    {
                        // Inserting new record into the Waiter table (with default values)
                        await _waiterRepository.Insert(new Waiter { User_ID = cur_user_id, Hours = (decimal)40.0f, Type = "Full" });
                    }
                    else
                    {
                        // Inserting new record into the Manager table (with default values)
                        await _managerRepository.Insert(new Manager { User_ID = cur_user_id, Area = "" });
                    }
                }
                else
                {
                    return BadRequest("To insert an employee you need to provide a subcategory\n");
                }

                // Displaying OK message
                string format1 = "The record with key={0} was added succesfully to the User and {1} tables\n";
                string format2 = "The record with key={0} was added succesfully to the User, {1}, and {2} tables\n";
                if (category.Equals("customer"))
                    return Ok(String.Format(format1, cur_user_id, textInfo.ToTitleCase(category.ToLower())));
                else
                    return Ok(String.Format(format2, cur_user_id, textInfo.ToTitleCase(category.ToLower()), textInfo.ToTitleCase(subcategory.ToLower())));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown errors
                return BadRequest("Error: Record was not inserted\n");
            }
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] User user)
        {
            // If the id in the body does not match the id in the URL
            if (id != user.ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Search record in the User table
                var response = await _repository.GetById(id);

                // If record was not found 
                if (response == null)
                {
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was was, moodify it 
                    await _repository.ModifyById(user);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, id));
                }

            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Record could not be updated\n");
            }
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Searching for record in the User table
                var response = await _repository.GetById(id);

                // Deleting record from the table
                await _repository.DeleteById(id);
                string format = "Record with key={0} deleted succesfully\n";
                return Ok(string.Format(format, id));
            }
            catch (Npgsql.PostgresException ex)
            {
                // Postgres threw an exception
                return BadRequest(ex.Message.ToString());
            }
            catch
            {
                // Unknown error
                return BadRequest("Error: Record could not be deleted\n");
            }
        }

        // api/user/getLastInserted
        [Route("getLastInserted")]
        [HttpGet]
        public async Task<ActionResult> getLastInserted()
        {
            try
            {
                // Getting the id of the last inserted user
                string format = "The last inserted user has id={0}\n";
                return Ok(string.Format(format, await _repository.getLastInsertedID()));
            }
            catch
            {
                // Some unknown error occurred
                return BadRequest("ERROR: Unable to get the id of the last inserted user\n");
            }
        }
    }
}

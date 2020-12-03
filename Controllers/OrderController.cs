using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Data;
using System.Threading.Tasks;
using System.Globalization;

namespace RestaurantAPI.Controllers
{
    /*
    Class used to describe orders that are not attached to a transaction.
    We initially create an order and an default transaction.
    This transaction would be later updated to contain the actual amount
    spent in dollars and time of the transaction (through a PUT operation possibly).
    We do this as we know that customer will always pay for an order. We also have to option
    to link the order to an existing transaction.
    */
    public class Order_No_Transaction
    {
        public int Order_ID { get; set; }
        public int User_ID { get; set; }
        public DateTime Date_Time { get; set;}
    }

    [Route("api/[controller]")]
    public class OrderController : Controller
    {

        private readonly OrderRepository _repository;
        private readonly In_Store_OrderRepository _in_store_orderRepository;
        private readonly Online_OrderRepository _online_orderRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly Customer_TransactionRepository _customer_TransactionRepository;
        private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public OrderController(OrderRepository repository, In_Store_OrderRepository in_store_orderRepository, Online_OrderRepository online_orderRepository, TransactionRepository transactionRepository, Customer_TransactionRepository customer_TransactionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _in_store_orderRepository = in_store_orderRepository ?? throw new ArgumentNullException(nameof(in_store_orderRepository));
            _online_orderRepository = online_orderRepository ?? throw new ArgumentNullException(nameof(online_orderRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _customer_TransactionRepository = customer_TransactionRepository ?? throw new ArgumentNullException(nameof(customer_TransactionRepository));
        }

        // GET: api/order
        [HttpGet]
        public async Task<List<Order>> Get()
        {
            // Getting all records from the Order tables
            return await _repository.GetAll();
        }

        // GET api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            try
            {
                // Searching for record in the database
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

        // POST api/order/in_store/6
        [Route("in_store/{tableno}/{tran_id?}")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order_No_Transaction order_no_transaction, int tableno, int? tran_id=null)
        {
            try
            {
                // TODO: Check if tran_id exists in the transaction table and see if you can play with the price
                // TODO: Validate table_no

                // Opening a transaction for the order (the transactionn will be modified and finalized once a customer pays) if no tran_id provided
                if (tran_id == null)
                {
                    Transaction def_tran = new Transaction { Amount = (decimal)0.0, Date_Time = order_no_transaction.Date_Time };
                    // Inserting default transaction into the Transaction table
                    await _transactionRepository.Insert(def_tran);

                    // Inserting transaction into Customer_Transaction table
                    int last_tran_inserted = await _transactionRepository.getLastTransactionInserted();
                    await _customer_TransactionRepository.Insert(new Customer_Transaction { User_ID = order_no_transaction.User_ID, Transaction_ID = last_tran_inserted });

                    // Inserting record in the Order table
                    await _repository.Insert(new Order { Order_ID = order_no_transaction.Order_ID, User_ID = order_no_transaction.User_ID, Transaction_ID=last_tran_inserted,Date_Time = order_no_transaction.Date_Time });
                }
                else // The id of an existing transaction was passed, so we just add orders to that transaction.
                {
                    // Inserting record in the Order table
                    await _repository.Insert(new Order { Order_ID = order_no_transaction.Order_ID, User_ID = order_no_transaction.User_ID, Transaction_ID=(int)tran_id, Date_Time = order_no_transaction.Date_Time });
                }

                // Getting last inserted order from Order table
                int last_order_id = await _repository.getLastOrderInserted();
                await _in_store_orderRepository.Insert(new In_Store_Order { Order_ID = last_order_id, TableNo=tableno });

                if (tran_id == null)
                {
                    return Ok("Records inserted successfully in the Order, In_Store_Order, and Transaction Tables");
                }
                else
                {
                    return Ok("Records inserted successfully in the Order and In_Store_Order tables\n");
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
                return BadRequest("Error: Record was not inserted\n");
            }
        }

        // POST api/order/online/uber eats
        [Route("online/{application}/{tran_id?}")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order_No_Transaction order_no_transaction, string application, int? tran_id=null)
        {
            // Making sure application name is in title case (just for convention)
            application = textInfo.ToTitleCase(application.ToLower());

            try
            {
                //TODO: Check if tran_id actually exsits in the transaction table and see if you can play with the price

                // Opening a transaction for the order (the transactionn will be modified and finalized once a customer pays) if no tran_id provided
                if (tran_id == null)
                {
                    Transaction def_tran = new Transaction { Amount = (decimal)0.0, Date_Time = order_no_transaction.Date_Time };
                    // Inserting default transaction into the Transaction table
                    await _transactionRepository.Insert(def_tran);

                    // Inserting transaction into Customer_Transaction table
                    int last_tran_inserted = await _transactionRepository.getLastTransactionInserted();
                    await _customer_TransactionRepository.Insert(new Customer_Transaction { User_ID = order_no_transaction.User_ID, Transaction_ID = last_tran_inserted });

                    // Inserting record in the Order table
                    await _repository.Insert(new Order { Order_ID = order_no_transaction.Order_ID, User_ID = order_no_transaction.User_ID, Transaction_ID = last_tran_inserted, Date_Time = order_no_transaction.Date_Time });
                }
                else // The id of an existing transaction was passed, so we just add orders to that transaction.
                {
                    // Inserting record in the Order table
                    await _repository.Insert(new Order { Order_ID = order_no_transaction.Order_ID, User_ID = order_no_transaction.User_ID, Transaction_ID = (int)tran_id, Date_Time = order_no_transaction.Date_Time });
                }

                // Getting last inserted order from Order table
                int last_order_id = await _repository.getLastOrderInserted();
                await _online_orderRepository.Insert(new Online_Order { Order_ID = last_order_id, Application = application });

                if (tran_id == null)
                {
                    return Ok("Records inserted successfully in the Order, Online_Order, and Transaction Tables");
                }
                else
                {
                    return Ok("Records inserted successfully in the Order and Online_Order tables\n");
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
                return BadRequest("Error: Record was not inserted\n");
            }
        }

        // PUT api/order/5
        [HttpPut("{order_id}")]
        public async Task<ActionResult> Put(int order_id, [FromBody] Order order)
        {
            // If id in body does not match id in URL
            if (order_id != order.Order_ID)
            {
                return BadRequest("id in URL has to match the id of the record to be updated\n");
            }

            try
            {
                // Searching for record in the database
                var response = await _repository.GetById(order_id);

                if (response == null)
                {
                    // If record does not exists
                    return NotFound("Record was not found\n");
                }
                else
                {
                    // If record was found modify it
                    await _repository.ModifyById(order);
                    string format = "The record with key={0} was updated succesfully\n";
                    return Ok(String.Format(format, order_id));
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
                return BadRequest("Error: Record scould not be updated\n");
            }
        }

        // DELETE api/order/5
        [HttpDelete("{order_id}")]
        public async Task<ActionResult> Delete(int order_id)
        {
            try
            {
                // Searching for record in the Order table
                var response = await _repository.GetById(order_id);

                // Erasing Order and Transaction
                if (await _repository.numOrderByTransaction(response.Transaction_ID) == 1)
                {
                    // Deleting records from Order table and Transaction table
                    await _repository.DeleteById(order_id);
                    await _transactionRepository.DeleteById(response.Transaction_ID);
                    string format = "Order with key={0} and Transaction with key={1} deleted succesfully from Order and Transaction tables\n";
                    return Ok(string.Format(format, order_id));
                }
                else
                {
                    // Deleting record from Order table
                    await _repository.DeleteById(order_id);
                    string format = "Order with key={0} deleted succesfully from Order table\n";
                    return Ok(string.Format(format, order_id));
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
                return BadRequest("Error: Record could not be deleted\n");
            }
        }

        // api/order/getLastInserted
        [Route("getLastInserted")]
        [HttpGet]
        public async Task<ActionResult> getLastInsertedOrderID()
        {
            try
            {
                // Getting all the id of the last inserted order from the Order table
                string format = "The id of the order last inserted order is {0}\n";
                return Ok(string.Format(format,await _repository.getLastOrderInserted()));
            }
            catch
            {
                // Some unknown error occurred
                return BadRequest("ERROR: Unable to get the id of the last inserted order\n");
            }
        }

        // api/order/numOrdersPerTransaction/5
        [Route("numOrdersByTransaction/{tran_id}")]
        [HttpGet]
        public async Task<ActionResult> getNumOrdersPerTransaction(int tran_id)
        {
            try
            {
                // Getting the number of orders for the specific transaction
                string format = "The number of orders for transaction with id={0} is/are {1}\n";
                return Ok(string.Format(format, tran_id, await _repository.numOrderByTransaction(tran_id)));
            }
            catch
            {
                // Some unknown error occurred
                return BadRequest("ERROR: Unable to get the number of orders for that transaction\n");
            }
        }
    }
}

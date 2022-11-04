using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERental.EFCore;
using ERental.Entities;
using ERental.BL;
using Microsoft.AspNetCore.Authorization;

namespace ERental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerWebAPIController : ControllerBase
    {
        private readonly CustomerBL customerBL = new CustomerBL();
        public CustomerWebAPIController()
        {

        }

        // GET: api/Customer
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            /* if (_context.Customers == null)
             {
                 return NotFound();
             }
             return await _context.Customers.ToListAsync();*/
            return new ActionResult<IEnumerable<Customer>>(customerBL.GetCustomers());
        }


        // GET: api/Customer/5
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var customer = customerBL.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


        // PUT: api/Customer/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                customerBL.UpdateCustomer(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Customer
        [HttpPost]
        //[Authorize]
        public ActionResult<Customer> PostCustomer(Customer customer)
        {
            //_context.Customers.Add(customer);
            try
            {
                customerBL.CreateCustomer(customer);
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);

        }


        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<Customer> DeleteCustomer(int id)
        {
            var customer = customerBL.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            customerBL.DeleteCustomer(customer.CustomerId);

            return NoContent();
        }


        private bool CustomerExists(int id)
        {
            if (customerBL.GetCustomer(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

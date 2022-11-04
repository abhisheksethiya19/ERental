using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.Entities;
using ERental.EFCore;

namespace ERental.DAL
{
    public class CustomerDAL
    {
        ERentalContext _context = new ERentalContext();

        public void CreateCustomer(Customer objCustomer)
        {
            _context.Add(objCustomer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer objCustomer)
        {
            _context.Entry(objCustomer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            Customer objCustomer = _context.Customers.Find(id);
            _context.Remove(objCustomer);
            _context.SaveChanges();
        }

        public Customer GetCustomer(int id)
        {
            Customer objCustomer = _context.Customers.Find(id);
            return objCustomer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }
    }
}

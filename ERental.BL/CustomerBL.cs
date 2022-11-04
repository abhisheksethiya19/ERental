using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.DAL;
using ERental.Entities;

namespace ERental.BL
{
    public class CustomerBL
    {
        CustomerDAL objCustomerDAL = new CustomerDAL();

        public void CreateCustomer(Customer objCustomer)
        {
            objCustomerDAL.CreateCustomer(objCustomer);
        }

        public void UpdateCustomer(Customer objCustomer)
        {
            objCustomerDAL.UpdateCustomer(objCustomer);
        }

        public void DeleteCustomer(int id)
        {
            objCustomerDAL.DeleteCustomer(id);
        }

        public Customer GetCustomer(int id)
        {
            Customer objCustomer = objCustomerDAL.GetCustomer(id);
            return objCustomer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return objCustomerDAL.GetCustomers();
        }
    }
}

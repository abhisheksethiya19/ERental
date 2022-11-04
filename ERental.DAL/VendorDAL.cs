using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.Entities;
using ERental.EFCore;

namespace ERental.DAL
{
    public class VendorDAL
    {
        ERentalContext _context = new ERentalContext();

        public void CreateVendor(Vendor objVendor)
        {
            _context.Add(objVendor);
            _context.SaveChanges();
        }

        public void UpdateVendor(Vendor objVendor)
        {
            _context.Entry(objVendor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteVendor(int id)
        {
            Vendor objVendor = _context.Vendors.Find(id);
            _context.Remove(objVendor);
            _context.SaveChanges();
        }

        public Vendor GetVendor(int id)
        {
            Vendor objVendor = _context.Vendors.Find(id);
            return objVendor;
        }

        public IEnumerable<Vendor> GetVendors()
        {
            return _context.Vendors;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.DAL;
using ERental.Entities;

namespace ERental.BL
{
    public class VendorBL
    {
        VendorDAL objVendorDAL = new VendorDAL();

        public void CreateVendor(Vendor objVendor)
        {
            objVendorDAL.CreateVendor(objVendor);
        }

        public void UpdateVendor(Vendor objVendor)
        {
            objVendorDAL.UpdateVendor(objVendor);
        }

        public void DeleteVendor(int id)
        {
            objVendorDAL.DeleteVendor(id);
        }

        public Vendor GetVendor(int id)
        {
            Vendor objVendor = objVendorDAL.GetVendor(id);
            return objVendor;
        }

        public IEnumerable<Vendor> GetVendors()
        {
            return objVendorDAL.GetVendors();
        }
    }
}

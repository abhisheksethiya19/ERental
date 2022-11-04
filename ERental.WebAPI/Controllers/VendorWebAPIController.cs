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
    public class VendorWebAPIController : ControllerBase
    {
        private readonly VendorBL vendorBL = new VendorBL();
        public VendorWebAPIController()
        {

        }

        // GET: api/VendorWebAPI
        [HttpGet]
        public ActionResult<IEnumerable<Vendor>> GetVendors()
        {
            return new ActionResult<IEnumerable<Vendor>>(vendorBL.GetVendors());
        }


        // GET: api/VendorWebAPI/5
        [HttpGet("{id}")]
        public ActionResult<Vendor> GetVendor(int id)
        {
            var vendor = vendorBL.GetVendor(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendor/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return BadRequest();
            }

            try
            {
                //await _context.SaveChangesAsync();
                vendorBL.UpdateVendor(vendor);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendor
        [HttpPost]
        //[Authorize]
        public ActionResult<Vendor> PostVendor(Vendor vendor)
        {
            //_context.Vendors.Add(vendor);
            try
            {
                vendorBL.CreateVendor(vendor);
            }
            catch (DbUpdateException)
            {
                if (VendorExists(vendor.VendorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetVendor", new { id = vendor.VendorId }, vendor);

        }

        // DELETE: api/Vendor/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<Vendor> DeleteVendor(int id)
        {
            var vendor = vendorBL.GetVendor(id);
            if (vendor == null)
            {
                return NotFound();
            }

            vendorBL.DeleteVendor(vendor.VendorId);

            return NoContent();
        }


        private bool VendorExists(int id)
        {
            if (vendorBL.GetVendor(id) != null)
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

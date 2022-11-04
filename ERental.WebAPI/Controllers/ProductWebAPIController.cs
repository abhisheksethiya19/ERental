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
    public class ProductWebAPIController : ControllerBase
    {
        private readonly ProductBL productBL = new ProductBL();

        public ProductWebAPIController()
        {

        }

        // GET: api/ProductWebAPI
        [HttpGet]
        //[Authorize]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            //if (_context.Products == null)
            //{
            //    return NotFound();
            //}
            return new ActionResult<IEnumerable<Product>>(productBL.GetProducts());
        }

        // GET: api/ProductWebAPI/5
        [HttpGet("{id}")]
        //[Authorize]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = productBL.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/ProductWebAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                productBL.updateProduct(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/ProductWebAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //if (_context.Products == null)
            //{
            //    return Problem("Entity set 'ERentalContext.Products'  is null.");
            //}
            try
            {
                productBL.CreateProduct(product);
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/ProductWebAPI/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<Product> DeleteProduct(int id)
        {
            //if (_context.Products == null)
            //{
            //    return NotFound();
            //}
            var product = productBL.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            productBL.DeleteProduct(product.ProductId);

            //return NoContent();
            return product;
        }

        private bool ProductExists(int id)
        {
            if (productBL.GetProduct(id) != null)
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

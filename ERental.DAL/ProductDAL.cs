using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.Entities;
using ERental.EFCore;

namespace ERental.DAL
{
    public class ProductDAL
    {
        ERentalContext _context = new ERentalContext();

        public void CreateProduct(Product objProduct)
        {
            _context.Add(objProduct);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product objProduct)
        {
            _context.Entry(objProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product objProduct = _context.Products.Find(id);
            _context.Remove(objProduct);
            _context.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            Product objProduct = _context.Products.Find(id);
            return objProduct;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
    }
}

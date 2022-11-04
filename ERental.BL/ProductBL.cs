using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.DAL;
using ERental.Entities;

namespace ERental.BL
{
    public class ProductBL
    {
        ProductDAL objProductDAL = new ProductDAL();

        public void CreateProduct(Product objProduct)
        {
            objProductDAL.CreateProduct(objProduct);
        }

        public void updateProduct(Product objProduct)
        {
            objProductDAL.UpdateProduct(objProduct);
        }

        public void DeleteProduct(int id)
        {
            objProductDAL.DeleteProduct(id);
        }

        public Product GetProduct(int id)
        {
            Product objProduct = objProductDAL.GetProduct(id);
            return objProduct;
        }

        public IEnumerable<Product> GetProducts()
        {
            return objProductDAL.GetProducts();
        }
    }
}

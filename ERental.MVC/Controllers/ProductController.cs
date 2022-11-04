using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERental.EFCore;
using ERental.Entities;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ERental.MVC.Controllers
{
    public class ProductController : Controller
    {
        //string httpLH = "https://localhost:7034";
        string httpLH = "https://localhost:44311";

        public ProductController()
        {

        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            //List<Product> objProduct = await GetProducts();                 // what is await      
            //return View(objProduct);


            List<Product> objProduct = await GetProducts();
            List<Vendor> objVendor = await GetVendors();

            var handler = new JwtSecurityTokenHandler();

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            var token = handler.ReadJwtToken(token2);

            var role = token.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();

            var result = from p in objProduct
                         join v in objVendor on p.VendorId equals v.VendorId
                         where v.UserName == role.Value
                         select p;
            return View(result);
        }

        // GET: Product/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Create()
        {
            Product product = new Product();
            Task<List<Vendor>> taskVendors = GetVendors();
            List<Vendor> Vendors = taskVendors.Result;
            ViewData["VendorId"] = new SelectList(Vendors, "VendorId", "FirstName", product.VendorId);

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductType,Description,PricePerMonth,InitialDeposit,FreeDelivery,IsAvailable,VendorId,DeliveryBy")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddProduct(product);
                }
                catch (Exception)
                {
                    if (ProductExists(product.ProductId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            Product objproduct = new Product();
            Task<List<Vendor>> taskVendors = GetVendors();
            List<Vendor> Vendors = taskVendors.Result;
            ViewData["VendorId"] = new SelectList(Vendors, "VendorId", "FirstName", objproduct.VendorId);

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductType,Description,PricePerMonth,InitialDeposit,FreeDelivery,IsAvailable,VendorId,DeliveryBy")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ProductExists(product.ProductId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await DeleteProduct(id);
            }
            catch (Exception)
            {
                throw;
            }
            //await DeleteProduct(id);
            return RedirectToAction(nameof(AdminProductList));
        }

        private async Task<Product> ProductExists(int id)
        {
            Product objProduct = await GetProduct(id);
            return objProduct;
        }

        //features available for admin

        // admin product list page
        public async Task<IActionResult> AdminProductList()
        {
            List<Product> objProduct = await GetProducts();                 // what is await      
            return View(objProduct);
        }

        // Customer product list page
        public async Task<IActionResult> CustomerProductList()
        {
            List<Product> objProduct = await GetProducts();                 // what is await      
            return View(objProduct);
        }

        // Rent 
        public async Task<IActionResult> Rent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            int item = ((int)product.PricePerMonth) + ((int)product.InitialDeposit);
            ViewData["Item"] = item;

            return View(product);
        }

        //product payment
        //[HttpPost]
        //public ActionResult ProductPayment(string noMonth, string noPro)
        //{
        //    decimal noM = Convert.ToDecimal(noMonth);
        //    decimal noP = Convert.ToDecimal(txtRate);

        //    decimal simpleinterest = (principleamount + rate + time) / 100;

        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("<b> amount :</b>" + principleamount + "<br/>");
        //    sb.Append("<b> Rate :</b>" + rate + "<br/>");
        //    sb.Append("<b> Time :</b>" + time + "<br/>");
        //    sb.Append("<b> Interest :</b>" + simpleinterest + "<br/>");

        //    return Content(sb.ToString());
        //}

        // product list for all
        public async Task<IActionResult> CommonProductList()
        {
            List<Product> objProduct = await GetProducts();                 // what is await      
            return View(objProduct);
        }

        // all searches 
        public async Task<IActionResult> Search(string? Name)
        {
            List<Product> objProduct = await GetProducts();
            var result = from prt in objProduct
                         where prt.ProductName == Name
                         select prt;
            if (result == null)
            {
                NotFound();
            }

            return View(result);    
        }

        public async Task<IActionResult> SearchByType(string? Name)
        {
            List<Product> objProduct = await GetProducts();
            var result = from prt in objProduct
                         where prt.ProductType == Name
                         select prt;
            if (result == null)
            {
                NotFound();
            }

            return View(result);
        }

        public async Task<IActionResult> SearchByPrice(decimal? price)
        {
            List<Product> objProduct = await GetProducts();
            var result = from prt in objProduct
                         where prt.PricePerMonth <= price
                         select prt;
            if (result == null)
            {
                NotFound();
            }

            return View(result);
        }

        // Order 
        public async Task<IActionResult> Order()
        {
            return View();
        }

        //------------------------------------------------------------------------//

        public async Task<List<Product>> GetProducts()
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/ProductWebAPI");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<Product> data = JsonConvert.DeserializeObject<List<Product>>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {
                return data;
            }

            return data;
        }

        public async Task<Product> GetProduct(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/ProductWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Product data = JsonConvert.DeserializeObject<Product>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {

                return data;
            }

            return data;
        }

        public async Task<Product> AddProduct(Product product)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(product);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/ProductWebAPI/", contentData);

            if (response.IsSuccessStatusCode)
            {
                return product;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(product);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("api/ProductWebAPI/" + product.ProductId, contentData);

            if (response.IsSuccessStatusCode)
            {
                return product;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return product;
        }

        public async Task<Product> DeleteProduct(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.DeleteAsync("/api/ProductWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Product data = JsonConvert.DeserializeObject<Product>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {
                return data;
            }
            return data;
        }

        // for dropdownlist
        public async Task<List<Vendor>> GetVendors()
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/VendorWebAPI");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<Vendor> data = JsonConvert.DeserializeObject<List<Vendor>>(stringData);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {
                return data;
            }

            return data;
        }
    }
}

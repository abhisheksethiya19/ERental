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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ERental.MVC.Controllers
{
    public class CustomerController : Controller
    {
        //string httpLH = "https://localhost:7034";
        string httpLH = "https://localhost:44311";

        public CustomerController()
        {

        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<Customer> objCustomer = await GetCustomers();                 // what is await      
            //return View(objCustomer);

            var handler = new JwtSecurityTokenHandler();

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            var token = handler.ReadJwtToken(token2);

            var role = token.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();

            var result = from c in objCustomer
                         where c.UserName == role.Value
                         select c;
            return View(result);
        }

        // GET: Customer/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        public IActionResult Create()
        {
            Customer customer = new Customer();
            Task<List<City>> taskCities = GetCitys();
            List<City> Cities = taskCities.Result;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName", customer.CityId);

            Task<List<State>> taskStates = GetStates();
            List<State> States = taskStates.Result;
            ViewData["StateId"] = new SelectList(States, "StateId", "StateName", customer.StateId);

            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,UserName,FirstName,LastName,StateId,CityId,PinCode,PhoneNo,EmailId,AccountBalance")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddCustomer(customer);
                }
                catch (Exception)
                {
                    if (CustomerExists(customer.CustomerId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Home");
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            Customer objcustomer = new Customer();

            Task<List<City>> taskCities = GetCitys();
            List<City> Cities = taskCities.Result;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName", objcustomer.CityId);

            Task<List<State>> taskStates = GetStates();
            List<State> States = taskStates.Result;
            ViewData["StateId"] = new SelectList(States, "StateId", "StateName", objcustomer.StateId);

            return View(customer);
        }

        // POST: customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,UserName,FirstName,LastName,StateId,CityId,PinCode,PhoneNo,EmailId,AccountBalance")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CustomerExists(customer.CustomerId) == null)
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

            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await DeleteCustomer(id);
            }
            catch (Exception)
            {
                throw;
            }
            //await DeleteCustomer(id);
            return RedirectToAction(nameof(AdminCustomerList));
        }

        private async Task<Customer> CustomerExists(int id)
        {
            Customer objCustomer = await GetCustomer(id);
            return objCustomer;
        }

        //features available for admin
        public async Task<IActionResult> AdminCustomerList()
        {
            List<Customer> objCustomer = await GetCustomers();                 
            return View(objCustomer);
        }

        //features available for admin
        public async Task<IActionResult> VendorCustomerList()
        {
            List<Customer> objCustomer = await GetCustomers();                
            return View(objCustomer);
        }

        // Account Balance
        public async Task<IActionResult> AccountBalance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            Customer objcustomer = new Customer();

            Task<List<City>> taskCities = GetCitys();
            List<City> Cities = taskCities.Result;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName", objcustomer.CityId);

            Task<List<State>> taskStates = GetStates();
            List<State> States = taskStates.Result;
            ViewData["StateId"] = new SelectList(States, "StateId", "StateName", objcustomer.StateId);

            return View(customer);
        }

        //EditBalance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBalance(int bAmount, [Bind("CustomerId,UserName,FirstName,LastName,StateId,CityId,PinCode,PhoneNo,EmailId,AccountBalance")] Customer customer)  //bAmount
        {
            //if (id != customer.CustomerId)
            //{
            //    return NotFound();
            //}

            customer.AccountBalance = customer.AccountBalance + bAmount;

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CustomerExists(customer.CustomerId) == null)
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

            return View(customer);
        }
        //------------------------------------------------------------------------//

        public async Task<List<Customer>> GetCustomers()
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/CustomerWebAPI");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<Customer> data = JsonConvert.DeserializeObject<List<Customer>>(stringData);

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

        public async Task<Customer> GetCustomer(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/CustomerWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Customer data = JsonConvert.DeserializeObject<Customer>(stringData);

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

        public async Task<Customer> AddCustomer(Customer customer)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(customer);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/CustomerWebAPI/", contentData);

            if (response.IsSuccessStatusCode)
            {
                return customer;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(customer);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("api/CustomerWebAPI/" + customer.CustomerId, contentData);

            if (response.IsSuccessStatusCode)
            {
                return customer;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return customer;
        }

        public async Task<Customer> DeleteCustomer(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.DeleteAsync("/api/CustomerWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Customer data = JsonConvert.DeserializeObject<Customer>(stringData);

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
        public async Task<List<City>> GetCitys()
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            HttpResponseMessage response = await client.GetAsync("/api/CityStateWebAPI/City");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<City> data = JsonConvert.DeserializeObject<List<City>>(stringData);

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

        public async Task<List<State>> GetStates()
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            HttpResponseMessage response = await client.GetAsync("/api/CityStateWebAPI/State");
            string stringData = response.Content.ReadAsStringAsync().Result;
            List<State> data = JsonConvert.DeserializeObject<List<State>>(stringData);

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

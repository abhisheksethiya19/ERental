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
    public class VendorController : Controller
    {
        //string httpLH = "https://localhost:7034";
        string httpLH = "https://localhost:44311";

        public VendorController()
        {

        }

        // GET: Vendor
        public async Task<IActionResult> Index()
        {
            List<Vendor> objVendor = await GetVendors();                 // what is await      
            //return View(objVendor);
            var handler = new JwtSecurityTokenHandler();

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            var token = handler.ReadJwtToken(token2);

            var role = token.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();

            var result = from v in objVendor
                         where v.UserName == role.Value
                         select v;
            return View(result);
        }

        // GET: Vendor/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await GetVendor(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }
        public IActionResult Create()
        {
            Vendor vendor = new Vendor();
            Task<List<City>> taskCities = GetCitys();
            List<City> Cities = taskCities.Result;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName", vendor.CityId);

            Task<List<State>> taskStates = GetStates();
            List<State> States = taskStates.Result;
            ViewData["StateId"] = new SelectList(States, "StateId", "StateName", vendor.StateId);

            return View();
        }

        // POST: Vendor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,UserName,FirstName,LastName,Address,StateId,CityId,PinCode,PhoneNo,EmailId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await AddVendor(vendor);
                }
                catch (Exception)
                {
                    if (VendorExists(vendor.VendorId) == null)
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

            return View(vendor);
        }

        // GET: Vendor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await GetVendor(id);
            if (vendor == null)
            {
                return NotFound();
            }

            Vendor objVendor = new Vendor();

            Task<List<City>> taskCities = GetCitys();
            List<City> Cities = taskCities.Result;
            ViewData["CityId"] = new SelectList(Cities, "CityId", "CityName", objVendor.CityId);

            Task<List<State>> taskStates = GetStates();
            List<State> States = taskStates.Result;
            ViewData["StateId"] = new SelectList(States, "StateId", "StateName", objVendor.StateId);

            return View(vendor);
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendorId,UserName,FirstName,LastName,Address,StateId,CityId,PinCode,PhoneNo,EmailId")] Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateVendor(vendor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (VendorExists(vendor.VendorId) == null)
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

            return View(vendor);
        }

        // GET: Vendor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vendor vendor = await GetVendor(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendor/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await DeleteVendor(id);
            }
            catch (Exception)
            {
                throw;
            }
            //await DeleteVendor(id);
            return RedirectToAction(nameof(AdminVendorList));
        }

        private async Task<Vendor> VendorExists(int id)
        {
            Vendor objVendor = await GetVendor(id);
            return objVendor;
        }

        //features available for admin
        public async Task<IActionResult> AdminVendorList()
        {
            List<Vendor> objVendor = await GetVendors();                 // what is await      
            return View(objVendor);
        }

        //features available for Customer
        public async Task<IActionResult> CustomerVendorList()
        {
            List<Vendor> objVendor = await GetVendors();                 // what is await      
            return View(objVendor);
        }

        //------------------------------------------------------------------------//

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

        public async Task<Vendor> GetVendor(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.GetAsync("/api/VendorWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Vendor data = JsonConvert.DeserializeObject<Vendor>(stringData);

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

        public async Task<Vendor> AddVendor(Vendor vendor)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            //var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(vendor);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/VendorWebAPI/", contentData);

            if (response.IsSuccessStatusCode)
            {
                return vendor;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return vendor;
        }

        public async Task<Vendor> UpdateVendor(Vendor vendor)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            string stringData = JsonConvert.SerializeObject(vendor);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("api/VendorWebAPI/" + vendor.VendorId, contentData);

            if (response.IsSuccessStatusCode)
            {
                return vendor;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return vendor;
        }

        public async Task<Vendor> DeleteVendor(int? id)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            HttpResponseMessage response = await client.DeleteAsync("/api/VendorWebAPI/" + id);
            string stringData = response.Content.ReadAsStringAsync().Result;
            Vendor data = JsonConvert.DeserializeObject<Vendor>(stringData);

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

using ERental.MVC.Models;
using ERental.WebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ERental.MVC.Controllers
{
    public class AuthenticateController : Controller
    {
        //string httpLH = "https://localhost:7034";
        string httpLH = "https://localhost:44311";
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginModel loginmodel)
        {
            var returnLoginModel = await AddLoginModel(loginmodel);


            var handler = new JwtSecurityTokenHandler();

            var token2 = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "token");
            var token = handler.ReadJwtToken(token2);

            var role = token.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            if ((role.Value == "Admin") ||
               (returnLoginModel.Username == "admin") && (returnLoginModel.Password == "Admin@123") ||
               (returnLoginModel.Username == "sir") && (returnLoginModel.Password == "Sir@123"))
            {
                return RedirectToAction("HomeAdmin", "Home");
            }
            else if (role.Value == "Vendor")
            {
                return RedirectToAction("HomeVendor", "Home");
            }
            else
            {
                return RedirectToAction("HomeCustomer", "Home");
            }

            //return RedirectToAction("Index", "Home");


            return View(loginmodel);
        }


        public async Task<LoginModel> AddLoginModel(LoginModel loginmodel)
        {
            string baseUrl = httpLH;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string stringData = JsonConvert.SerializeObject(loginmodel);
            var contentData = new StringContent(stringData,
        System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/AuthenticateWebAPI/login", contentData);

            if (response.IsSuccessStatusCode)
            {
                string stringJWT = response.Content.
                ReadAsStringAsync().Result;
                JWT jwt = JsonConvert.DeserializeObject
                <JWT>(stringJWT);

                //HttpContext.Session.SetString("token", jwt.Token);
                // HttSession["token"] = jwt.Token;
                //HttpContext.Session.SetString("token", jwt.Token);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "token", jwt.Token);

            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return loginmodel;

        }


        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Email,Password,Role")] RegisterModel registermodel)
        {
            var returnedregistermodel = await AddRegisterModel(registermodel);

            if (returnedregistermodel.Role == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (returnedregistermodel.Role == "Vendor")
            {
                return RedirectToAction("Create", "Vendor");
            }
            else
            {
                return RedirectToAction("Create", "Customer");
            }

            //return RedirectToAction("Index", "Home");
        }

        private object RegisterModelExists(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterModel> AddRegisterModel(RegisterModel registermodel)
        {
            string baseUrl = httpLH;

            HttpClient client = new HttpClient();
            if (registermodel.Role == "Admin")
            {
                registermodel.Role = UserRoles.ADMIN;
            }
            else if (registermodel.Role == "Vendor")
            {
                registermodel.Role = UserRoles.VENDOR;
            }
            else
            {
                registermodel.Role = UserRoles.CUSTOMER;
            }

            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string stringData = JsonConvert.SerializeObject(registermodel);
            var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/AuthenticateWebAPI/registeradmin", contentData);

            if (response.IsSuccessStatusCode)
            {
                return registermodel;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            return registermodel;
        }

        public IActionResult Logout()
        {
            return View();
        }
        public IActionResult ConfirmLogout()
        {
            HttpContext.Session.Remove("token");
            ViewBag.Message = "User logged out successfully!";
            //return View("Index");
            return RedirectToAction("Index", "Home");
        }
    }
}

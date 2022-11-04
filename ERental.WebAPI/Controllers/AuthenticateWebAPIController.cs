using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERental.WebAPI.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ERental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateWebAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        // Dependency injection
        public AuthenticateWebAPIController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

            //CreateRole();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                     issuer: _configuration["JWT:ValidIssuer"],
                     audience: _configuration["JWT:ValidAudience"],
                     expires: DateTime.Now.AddHours(150),
                     claims: authClaims,
                     signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                     );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            //CreateRole
            if (!await roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            if (!await roleManager.RoleExistsAsync(UserRoles.VENDOR))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.VENDOR));
            if (!await roleManager.RoleExistsAsync(UserRoles.CUSTOMER))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.CUSTOMER));

            if (model.Role == UserRoles.ADMIN)
            {
                if (await roleManager.RoleExistsAsync(UserRoles.ADMIN))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.ADMIN);
                }


            }
            else if (model.Role == UserRoles.VENDOR)
            {

                if (await roleManager.RoleExistsAsync(UserRoles.VENDOR))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.VENDOR);
                }
            }
            else if (model.Role == UserRoles.CUSTOMER)
            {
                if (await roleManager.RoleExistsAsync(UserRoles.CUSTOMER))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.CUSTOMER);
                }
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        public ActionResult<IEnumerable<IdentityRole>> GetRoles()
        {
            return new ActionResult<IEnumerable<IdentityRole>>(roleManager.Roles);

        }

        public async Task CreateRole()
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            if (!await roleManager.RoleExistsAsync(UserRoles.VENDOR))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.VENDOR));
            if (!await roleManager.RoleExistsAsync(UserRoles.CUSTOMER))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.CUSTOMER));
            return;
        }

    }
}

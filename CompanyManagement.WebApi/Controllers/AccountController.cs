using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompanyManagement.Core;
using CompanyManagement.Core.Entities;
using CompanyManagement.Core.HelperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = CompanyManagement.Core.HelperModels.Requests.LoginRequest;
using RegisterRequest = CompanyManagement.Core.HelperModels.Requests.RegisterRequest;

namespace CompanyManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Client> _userManager;
        private readonly SignInManager<Client> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly Jwt _jwt;

        public AccountController(UserManager<Client> userManager, SignInManager<Client> signInManager, IConfiguration configuration, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

            _jwt = jwt.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var user = new Client() { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ClientRolesEnum.RoleA.ToString());

                var token = await GenerateToken(user);
                return Ok(new
                {
                    message = "User created successfully",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await GenerateToken(user);
                return Ok(new
                {
                    message = "User successfully logged in.",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized("User/Pass does not match");
        }

        private async Task<JwtSecurityToken> GenerateToken(Client user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim("name", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("role", userRole));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                claims: authClaims,
                signingCredentials: signingCredentials
            );

            return token;
        }
    }
}

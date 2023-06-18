using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        private IConfiguration _config;
        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        //View all registered users. Only visible to admin.
        [HttpGet("AllUsers")]
        [Authorize(Policy = "admin")]
        public IEnumerable<User> GetAllUsers()
        {
            return _appdb.Users;
        }
        
        //Register a new user. Set value of ProfileId to 1 if admin and 2 if seller. Default value is set to 3 for customers. 
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] User user)
        {
            var userExists = _appdb.Users.FirstOrDefault(u => u.EmailId == user.EmailId);
            if (userExists != null)
            {
                return BadRequest("User with same email id already exists");
            }
            _appdb.Users.Add(user);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Login using EmailId and password to retrieve a JWT token and proceed.
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] User user)
        {
            var currentUser = _appdb.Users.FirstOrDefault(x => x.EmailId == user.EmailId && x.Password == user.Password);

            if (currentUser == null) { return NotFound(); }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            string profile = "";
            foreach (var role in _appdb.Profiles)
            {
                if (role.ProfileId == currentUser.ProfileId)
                {
                    profile = role.ProfileName;
                    break;
                }
            }

            var profileClaim = new Claim("ProfileType", profile); // Add user type claim
            var emailClaim = new Claim(ClaimTypes.Email, user.EmailId);
            
            var claims = new[] 
            {  
                profileClaim,
                emailClaim 
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }
    }
}




        //[HttpPost("[action]")]
        //
        //public IActionResult Logout()
        //{
        //    // Invalidate the token by setting its expiration time to a past date/time
        //    var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    var jwtHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

        //    var expiredToken = new JwtSecurityToken(
        //        _config["JWT:Issuer"],
        //        _config["JWT:Audience"],
        //        jwtToken.Claims,
        //        DateTime.Now,
        //        DateTime.Now.AddMinutes(-60),  // Expired token with negative expiration time
        //        jwtToken.SigningCredentials
        //    );

        //    var newToken = jwtHandler.WriteToken(expiredToken);

        //    return Ok("Logout successful");
        //}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementApplication.Models;
using System.Security.Claims;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        [HttpPost]
        [Authorize]
        //https://localhost:7252/api/bookings
        public IActionResult Post([FromBody] Booking booking)
        {
            if (booking == null)
                return NoContent();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            booking.UserId = user.Id;
            _appdb.Bookings.Add(booking);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

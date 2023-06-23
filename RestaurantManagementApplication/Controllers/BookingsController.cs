using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementApplication.DTO;
using RestaurantManagementApplication.Models;
using System.Security.Claims;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        //View all bookings made. Only visible to admin.
        [HttpGet("AllBookings")]
        [Authorize(Policy = "admin")]
        public IEnumerable<Booking> GetAllBookings()
        {
            return (IEnumerable<Booking>)_appdb.Bookings.GroupBy(u => u.UserId); //testing groupby method
        }
        
        //View all your bookings.
        [HttpGet("MyBookings")]
        [Authorize(Policy = "customer")]
        public IActionResult GetBookings()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var bookings = _appdb.Bookings.Where(b => b.UserId == user.Id).OrderByDescending(x => x.BookingDate).Select(
                b => new BookingDTO(b.Id, b.User.UserName, b.Pickup.Name, b.Pickup.Location, b.BookingDate.ToString("g")));

            if (bookings.IsNullOrEmpty())
                return NotFound("No bookings made.");
            else
                return Ok(bookings);
        }
        
        //Enter a BookingId to view that booking.
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,customer")]
        public IActionResult Get(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.FirstOrDefault(b => b.Id == id && (b.UserId == user.Id || user.ProfileId == 1));           
            if (booking == null)
                return NotFound($"Booking {id} not found.");

            var bookingdto = _appdb.Bookings.Where(b => b.Id == booking.Id).Select(
                b => new BookingDTO(b.Id, b.User.UserName, b.Pickup.Name, b.Pickup.Location, b.BookingDate.ToString("g")));

            return Ok(bookingdto);
        }

        //Create a booking by entering the location of your nearest pickup.
        [HttpPost]
        [Authorize(Policy = "customer")]
        public IActionResult Post([FromBody] string location) //to check if string location works or need to send object of Booking class
        {
            if (location == null)
                return BadRequest();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var pickup = _appdb.Pickups.FirstOrDefault(p => p.Location == location);
            if(pickup == null)
                return NotFound($"No pickup found at {location}.");
            
            Booking booking = new Booking();
            booking.UserId = user.Id;
            booking.PickupId = pickup.Id;
            booking.BookingDate = DateTime.Now;
            _appdb.Bookings.Add(booking);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Enter the BookingId of the booking you want to edit.          
        [HttpPut("{id}")]
        [Authorize(Policy = "customer")]
        public IActionResult Put(int id, [FromBody] string location) //to check if string location works or need to send object of Booking class
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var pickup = _appdb.Pickups.FirstOrDefault(p => p.Location == location);
            if(pickup == null)
                return NotFound($"No pickup found at {location}.");

            var booking = _appdb.Bookings.FirstOrDefault(b => b.Id == id && b.UserId == user.Id);
            if (booking == null)
                return NotFound($"Booking {id} not found.");

            booking.Pickup.Location = pickup.Location;
            booking.BookingDate = DateTime.Now;
            _appdb.SaveChanges();
            return Ok($"Booking {id} updated successfully!");
        }

        //Enter the BookingId of the booking you want to delete.
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,customer")]
        public IActionResult DeleteBooking(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.FirstOrDefault(b => b.Id == id && (b.UserId == user.Id || user.ProfileId == 1));
            if (booking == null)
                return NotFound($"Booking {id} not found.");

            _appdb.Bookings.Remove(booking);
            _appdb.SaveChanges();
            return Ok($"Booking {id} deleted successfully!");
        }
    }
}

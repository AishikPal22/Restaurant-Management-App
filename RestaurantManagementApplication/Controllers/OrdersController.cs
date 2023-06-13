using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementApplication.Models;
using System.Security.Claims;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        [HttpGet]
        [Authorize]
        //https://localhost:7252/api/orders
        public IActionResult Get()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);

            if (user == null)
                return NotFound();

            var booking = _appdb.Bookings.Where(c => c.UserId == user.Id);
            var order = _appdb.Orders.Where(o => o.BookingId == booking.Select(b=>b.Id));
            
            if (order == null)
                return BadRequest();
            else
                return Ok(order);
        }
        
        [HttpPost]
        [Authorize]
        //https://localhost:7252/api/orders
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
                return NoContent();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            var orderItem = _appdb.Menu.FirstOrDefault(x=>x.Name == order.ItemName);
            
            if (orderItem == null)
                return NoContent();
            
            order.ItemId = orderItem.Id;
            _appdb.Orders.Add(order);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

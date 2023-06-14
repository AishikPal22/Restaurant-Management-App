using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementApplication.DTO;
using RestaurantManagementApplication.Models;
using System.Security.Claims;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        [HttpGet("GetAllOrders")]
        [Authorize]
        //https://localhost:7252/api/orders
        public IActionResult GetAllOrders(int bookingId)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);

            if (user == null)
                return NotFound();

            var booking = _appdb.Bookings.FirstOrDefault(x => x.Id == bookingId && x.UserId == user.Id);

            if(booking == null) 
                return NotFound();

            var order = _appdb.Orders.Where(b => b.BookingId == booking.Id).Select(
                o => new OrderDTO
                {
                    //UserId = user.Id,
                    UserName = user.UserName,
                    BookingId = o.BookingId,
                    //BookingTime = o.Booking.BookingTime,
                    OrderId = o.Id,
                    ItemName = o.ItemName,
                    ItemDescription = o.Item.Description,
                    ItemPrice = o.Item.Price,
                    ItemQuantity = o.Quantity
                });
            
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

            var orderItem = _appdb.Menu.FirstOrDefault(x => x.Name == order.ItemName);

            if (orderItem == null)
                return NoContent();

            order.ItemId = orderItem.Id;
            order.Amount = order.Quantity * orderItem.Price;
            _appdb.Orders.Add(order);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

            //var booking = _appdb.Bookings.Where(c => c.UserId == user.Id);
            ////var order = _appdb.Orders.Where(o => o.BookingId == booking.Select(b=>b.Id));
            //var orderList = new List<Order>();
            //foreach (var order in _appdb.Orders) {
            //    foreach (var book in booking) {
            //        if (order.BookingId == book.Id) {
            //            orderList.Add(order);
            //        }
            //    }
            //}
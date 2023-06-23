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
    public class OrdersController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        //View all bookings made. Only visible to admin.
        [HttpGet("AllOrders")]
        [Authorize(Policy = "admin")]
        public IEnumerable<Order> GetAllOrders()
        {
            return (IEnumerable<Order>)_appdb.Orders.GroupBy(u => u.BookingId); //testing groupby method
        }
        
        //Enter a BookingId to view all orders for that booking.
        [HttpGet("MyOrders/{id}")]
        [Authorize(Roles = "admin, customer")]
        public IActionResult GetOrders(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            //int bookingid = int.Parse(id);
            var booking = _appdb.Bookings.FirstOrDefault(x => x.Id == id && (x.UserId == user.Id || user.ProfileId == 1));
            if (booking == null)
                return NotFound($"Booking {id} not found.");

            var orders = _appdb.Orders.OrderByDescending(o => o.Quantity).Where(b => b.BookingId == booking.Id).Select(
                o => new OrderDTO(o.Id, o.ItemName, o.Item.Price, o.Quantity, o.Price));

            if (orders.IsNullOrEmpty())
                return NotFound("No orders made.");
            else
                return Ok(orders);
        }

        //Order any item from menu.
        [HttpPost]
        [Authorize(Policy = "customer")]
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
                return NoContent();

            if (!string.IsNullOrEmpty(order.BookingId.ToString()) || !string.IsNullOrEmpty(order.Price.ToString()))
                return BadRequest("Posting objects with values for certain properties is not allowed.");

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var item = _appdb.Menu.FirstOrDefault(x => x.Name == order.ItemName && x.IsAvailable == true);
            if (item == null)
                return NotFound($"{order.ItemName} is not available.");

            var booking = _appdb.Bookings.OrderBy(i => i.BookingDate).LastOrDefault(x => x.UserId == user.Id);
            if (booking == null)
                return NotFound("Make a booking and then proceed to order.");

            order.BookingId = booking.Id;
            order.ItemId = item.Id;
            order.Price = order.Quantity * item.Price;
            _appdb.Orders.Add(order);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "customer")]
        //Enter the OrderId of the order you want to edit. 
        public IActionResult Put(int id, [FromBody] Order order)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.OrderBy(i => i.BookingDate).LastOrDefault(x => x.UserId == user.Id);
            if (booking == null)
                return NotFound("You haven't made any booking.");

            if (order == null)
                return NoContent();

            if (!string.IsNullOrEmpty(order.BookingId.ToString()) || !string.IsNullOrEmpty(order.Price.ToString()))
                return BadRequest("Posting objects with values for certain properties is not allowed.");

            var orderToUpdate = _appdb.Orders.FirstOrDefault(o => o.Id == id);
            if (orderToUpdate == null)
                return NotFound($"No order exists with OrderId {id}");

            if (orderToUpdate.BookingId == booking.Id)
                return BadRequest("Order does not exist in your current booking.");

            var item = _appdb.Menu.FirstOrDefault(x => x.Name == order.ItemName && x.IsAvailable == true);
            if (item == null)
                return NotFound($"{order.ItemName} is not available.");

            orderToUpdate.ItemName = order.ItemName;
            orderToUpdate.ItemId = item.Id;
            orderToUpdate.Quantity = order.Quantity;
            orderToUpdate.Price = order.Quantity * item.Price;
            _appdb.SaveChanges();
            return Ok($"Order {id} updated successfully!");
        }

        //Enter the OrderId of the order you want to delete.
        [HttpDelete("{id}")]
        [Authorize(Policy = "customer")]
        public IActionResult Delete(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.OrderBy(i => i.BookingDate).LastOrDefault(x => x.UserId == user.Id);
            if (booking == null)
                return NotFound("You haven't made any booking.");

            var orderToDelete = _appdb.Orders.FirstOrDefault(o => o.Id == id);
            if (orderToDelete == null)
                return NotFound($"No order exists with OrderId {id}");

            if (orderToDelete.BookingId == booking.Id)
                return BadRequest("Order does not exist in your current booking.");

            _appdb.Orders.Remove(orderToDelete);
            _appdb.SaveChanges();
            return Ok($"Order {id} deleted successfully!");
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
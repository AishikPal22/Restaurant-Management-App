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
    public class BillsController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        //View all bills processed. Only visible to admin.
        [HttpGet("AllBills")]
        [Authorize(Policy = "admin")]
        public IEnumerable<Bill> GetAllBills()
        {
            return (IEnumerable<Bill>)_appdb.Bills.GroupBy(u => u.UserId); //testing groupby method
        }

        //View bills for all your bookings.
        [HttpGet("MyBills")]
        [Authorize(Policy = "customer")]
        public IActionResult GetBills()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var bookings = _appdb.Bookings.Where(b => b.UserId == user.Id);
            if (bookings.IsNullOrEmpty())
                return NotFound("No bookings found.");

            var bills = _appdb.Bills.Where(b => b.UserId == user.Id);
            if (bills.IsNullOrEmpty())
                return NotFound("No bills found.");

            var billdto = bills.OrderBy(x => x.Id).Select(
                b => new BillDTO(b.Id,
                                b.User.UserName,
                                b.BookingId,
                                bookings.FirstOrDefault(f => f.Id == b.BookingId).Pickup.Name,
                                bookings.FirstOrDefault(f => f.Id == b.BookingId).Pickup.Location,
                                b.AllOrders,
                                b.Amount));

            return Ok(billdto);

        }

        //Enter the BookingId to view bill for that booking. 
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,customer")]
        public IActionResult Get(int bookingid)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.FirstOrDefault(b => b.Id == bookingid && b.UserId == user.Id);
            if (booking == null)
                return NotFound($"Booking {bookingid} not found.");

            var bill = _appdb.Bills.FirstOrDefault(b => b.BookingId == bookingid && (b.UserId == user.Id || user.ProfileId == 1));
            if (bill == null)
                return NotFound($"Bill for booking {bookingid} not found.");

            var billdto = _appdb.Bills.Where(x => x.Id == bill.Id).Select(
                b => new BillDTO(b.Id,
                                b.User.UserName,
                                b.BookingId,
                                booking.Pickup.Name,
                                booking.Pickup.Location,
                                b.AllOrders,
                                b.Amount));

            return Ok(billdto);

        }

        //Generate bill for your last booking.
        [HttpPost]
        [Authorize(Policy = "customer")]
        public IActionResult Post()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            if (user == null)
                return BadRequest();

            var booking = _appdb.Bookings.OrderBy(b => b.BookingDate).LastOrDefault(x => x.UserId == user.Id);
            if (booking == null)
                return NotFound("You haven't made any booking.");

            var bill = new Bill();
            bill.UserId = user.Id;
            bill.BookingId = booking.Id;
            List<string> Orders = new List<string>();
            foreach (var data in _appdb.Orders)
            {
                if (data.BookingId == bill.BookingId)
                {
                    Orders.Add(string.Concat(data.ItemName, "--", data.Quantity, "--", data.Price, "\n")); //String.Concat(data.ItemName,"\t",data.Quantity,"\t",data.Price)                   
                    bill.Amount += data.Price;
                }
            }
            bill.AllOrders = Orders.ToString();
            _appdb.Bills.Add(bill);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Enter the BookingId to delete bill for that booking. 
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public IActionResult Delete(int bookingid)
        {
            //If booking with entered BookingId not found, delete bill for the same automatically.
            var booking = _appdb.Bookings.FirstOrDefault(b => b.Id == bookingid);
            if (booking == null)
                _appdb.Bills.Remove(_appdb.Bills.FirstOrDefault(b => b.BookingId == booking.Id));

            var bill = _appdb.Bills.FirstOrDefault(b => b.BookingId == bookingid);
            if (bill == null)
                return NotFound($"Bill for booking {bookingid} not found.");

            _appdb.Bills.Remove(bill);
            _appdb.SaveChanges();
            return Ok($"Bill for booking {bookingid} deleted successfully!");
        }
    }
}


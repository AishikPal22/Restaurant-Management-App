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
    public class BillsController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        [HttpGet("GetBill")]
        [Authorize]
        //[Route("Action")]
        public IActionResult GetBill(int bookingid)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            var bills = _appdb.Bills.Where(b => b.UserId == user.Id);
            var bill = bills.FirstOrDefault(x => x.BookingId == bookingid);

            var billdto = _appdb.Bills.Where(z=>z.Id == bill.Id).Select(
                o => new BillDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    BookingId = o.BookingId,
                    Items = o.AllOrders.ToString(),
                    Amount = o.Amount
                });

            if (bill == null)
                return BadRequest();
            else
                return Ok(bill);

        }

        [HttpGet("GetAllBills")]
        [Authorize]
        //[Route("Action")]
        public IActionResult GetAllBills()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            var bills = _appdb.Bills.Where(b => b.UserId == user.Id);
            //var bill = bills.FirstOrDefault(x => x.BookingId == bookingid);

            var billdto = _appdb.Bills.Select(
                o => new BillDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    BookingId = o.BookingId,
                    Items = o.AllOrders.ToString(),
                    Amount = o.Amount
                });

            if (bills == null)
                return BadRequest();
            else
                return Ok(bills);

        }
        
        [HttpPost("{id}")]
        [Authorize]
        //[Route("Action")]
        public IActionResult Post(int bookingid)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            var bill = new Bill();
            bill.UserId = user.Id;
            bill.BookingId = bookingid;
            foreach(var data in _appdb.Orders)
            {
                if(data.BookingId == bill.BookingId)
                {
                    bill.AllOrders.Add(data.ItemId, data.ItemName);                    
                    bill.Amount += data.Amount;
                }
            }
            _appdb.Bills.Add(bill);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

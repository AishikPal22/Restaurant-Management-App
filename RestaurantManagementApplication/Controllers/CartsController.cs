using Microsoft.AspNetCore.Mvc;
using RestaurantManagementApplication.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        // GET: api/<CartsController>
        ApplicationDbContext _appdb = new ApplicationDbContext();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CartsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CartsController>
        [HttpPost]
        public void Post([FromBody] Cart cart)
        {
            _appdb.Carts.Add(cart);
            _appdb.SaveChanges();
        }

        // PUT api/<CartsController>/5
        [HttpPost("{id}")]
        public decimal ConfirmOrder(int id, int bookingid)
        {
            //var b = new Random();
            //int bookingid=b.Next();
            var c = _appdb.Carts.Where(x => x.UserId == id).ToList();
            decimal d = 0;
            foreach (var c2 in c)
            {
                var x = _appdb.Menu.Where(x => x.Id == c2.ItemId).FirstOrDefault();
                if (x != null)
                {
                    d = d + (x.Price * c2.Quantity);
                    Order o = new Order();
                    o.BookingId = bookingid;
                    o.Quantity = c2.Quantity;
                    o.ItemId = c2.ItemId;
                    o.ItemName = x.Name;
                    _appdb.Orders.Add(o);
                    _appdb.SaveChanges();
                }
            }
            foreach(var c2 in c)
            {
                _appdb.Carts.Remove(c2);
            }
            _appdb.SaveChanges();
            return d;
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

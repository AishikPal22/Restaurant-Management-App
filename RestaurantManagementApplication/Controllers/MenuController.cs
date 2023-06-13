using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementApplication.Models;
using System.Security.Claims;

namespace RestaurantManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        [HttpGet]
        [Authorize]
        //https://localhost:7252/api/menu
        public IActionResult Get()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();
            
            return Ok(_appdb.Menu);
        }

        [HttpPost]
        [Authorize]
        //https://localhost:7252/api/menu
        public IActionResult Post([FromBody] Item item)
        {
            if (item == null)
                return NoContent();
            
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();
            
            if (!user.IsAdmin)
                return BadRequest();
            
            _appdb.Menu.Add(item);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        [Authorize]
        //https://localhost:7252/api/menu/0
        public IActionResult Put(int id, [FromBody] Item item)
        {
            var update = _appdb.Menu.FirstOrDefault(p => p.Id == id);
            if (update == null)
                return NoContent();
            
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();
            
            if (!user.IsAdmin)
                return BadRequest();

            update.Name = item.Name;
            update.Description = item.Description;
            update.Price = item.Price;
            update.IsAvailable = item.IsAvailable;
            _appdb.SaveChanges();
            return Ok("Record updated successfully!");
        }

        [HttpDelete("{id}")]
        [Authorize]
        //https://localhost:7252/api/menu/0
        public IActionResult Delete(int id)
        {
            var delete = _appdb.Menu.FirstOrDefault(p => p.Id == id);
            
            if (delete == null) 
                return NoContent();
            
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _appdb.Users.FirstOrDefault(u => u.EmailId == userEmail);
            
            if (user == null)
                return NotFound();

            if (!user.IsAdmin)
                return BadRequest();

            _appdb.Menu.Remove(delete);
            _appdb.SaveChanges();
            return Ok("Record deleted successfully!");
        }
    }
}

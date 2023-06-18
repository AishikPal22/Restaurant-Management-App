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
    public class PickupsController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        //View all available pickups.
        [HttpGet]
        [Authorize(Roles = "admin, customer")]
        public IActionResult Get()
        {
            var pickups = _appdb.Pickups.DefaultIfEmpty().OrderBy(p => p.Name).Select(
                p => new PickupDTO(p.Name, p.ContactNo, p.HighwayNo, p.Location));

            if (pickups.IsNullOrEmpty())
                return NotFound("No pickup exists.");
            else
                return Ok(pickups);
        }

        //Add a new pickup.
        [HttpPost]
        [Authorize(Roles = "admin, seller")]
        public IActionResult Post([FromBody] Pickup pickup)
        {
            if (pickup == null)
                return NoContent();

            var pickupExists = _appdb.Pickups.FirstOrDefault(p => p.Location == pickup.Location);
            if (pickupExists != null)
                return BadRequest("Pickup at same location already exists.");

            _appdb.Pickups.Add(pickup);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Enter PickupId to update that pickup.
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public IActionResult Put(int id, [FromBody] Pickup pickup)
        {
            var update = _appdb.Pickups.FirstOrDefault(p => p.Id == id);
            if (update == null)
                return NotFound($"No pickup found with id {id}");

            if (pickup.Location == update.Location)
                return BadRequest("Pickup at same location already exists.");

            update.Name = pickup.Name;
            update.ContactNo = pickup.ContactNo;
            update.HighwayNo = pickup.HighwayNo;
            update.Location = pickup.Location;
            _appdb.SaveChanges();
            return Ok("Pickup info updated successfully!");
        }

        //Enter PickupId to delete that pickup.
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public IActionResult Delete(int id)
        {
            var pickup = _appdb.Pickups.FirstOrDefault(p => p.Id == id);
            if (pickup == null)
                return NotFound($"No pickup found with id {id}");

            _appdb.Pickups.Remove(pickup);
            _appdb.SaveChanges();
            return Ok($"Pickup: {pickup.Name} deleted.");
        }
    }
}

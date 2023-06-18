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
    public class MenuController : ControllerBase
    {
        ApplicationDbContext _appdb = new ApplicationDbContext();

        //View all available items from menu.
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var menu = _appdb.Menu.OrderBy(m => m.Name).Where(m => m.IsAvailable == true).Select(
                m => new MenuDTO(m.Name, m.Description, m.Price));

            if (menu.IsNullOrEmpty())
                return NotFound("No item exists.");
            else
                return Ok(menu);
        }

        //Add a new item to the menu.
        [HttpPost]
        [Authorize(Policy = "admin")]
        public IActionResult Post([FromBody] Item item)
        {
            if (item == null)
                return NoContent();

            var itemExists = _appdb.Menu.FirstOrDefault(m => m.Name == item.Name);
            if (itemExists != null)
                return BadRequest("Item with same name already exists.");

            item.IsAvailable = true;
            _appdb.Menu.Add(item);
            _appdb.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Enter ItemId to update that item from menu.
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public IActionResult Put(int id, [FromBody] Item item)
        {
            var update = _appdb.Menu.FirstOrDefault(p => p.Id == id);
            if (update == null)
                return NotFound($"No item found with id {id}");

            if (item.Name == update.Name)
                return BadRequest("Item with same name already exists.");

            update.Name = item.Name;
            update.Description = item.Description;
            update.Price = item.Price;
            update.IsAvailable = item.IsAvailable;
            _appdb.SaveChanges();
            return Ok("Item info updated successfully!");
        }

        //Enter ItemId to delete that item from menu.
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public IActionResult Delete(int id)
        {
            var item = _appdb.Menu.FirstOrDefault(p => p.Id == id);
            if (item == null)
                return NotFound($"No item found with id {id}");

            _appdb.Menu.Remove(item);
            _appdb.SaveChanges();
            return Ok($"Item: {item.Name} deleted successfully!");
        }
    }
}

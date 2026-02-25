using Checkpoint_12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Checkpoint_12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly Checkpoint_12DBContext _context;

        public UserController(Checkpoint_12DBContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == id);
            return user == null ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {

            return Ok(await _context.Users.Include(u => u.Profile).ToListAsync());

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = await _context.Users.Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            if (user.Profile?.Bio != null) existingUser.Profile.Bio = user.Profile.Bio;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
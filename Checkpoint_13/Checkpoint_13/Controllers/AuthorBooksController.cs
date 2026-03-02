using Checkpoint_13.Data;
using Microsoft.AspNetCore.Mvc;
using Checkpoint_13.Models;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint_13.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthorBooksController : ControllerBase
    {
        private readonly Checkpoint_13DBContext _context;

        public AuthorBooksController(Checkpoint_13DBContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var user = await _context.Authors.Include(u => u.Books)
                .FirstOrDefaultAsync(u => u.AuthorId == id);
            return user == null ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {

            return Ok(await _context.Authors.Include(u => u.Books).ToListAsync());

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
        {
            var existingAuthor = await _context.Authors
                .FirstOrDefaultAsync(u => u.AuthorId == id);
            if (existingAuthor == null) return NotFound();

            existingAuthor.Name = author.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var user = await _context.Authors.FindAsync(id);
            if (user == null) return NotFound();

            _context.Authors.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

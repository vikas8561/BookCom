using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookweb.Context;
using Bookweb.Models;

namespace Bookweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddBooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddBooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AddBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddBook>>> GetAddBook()
        {
          if (_context.AddBook == null)
          {
              return NotFound();
          }
            return await _context.AddBook.ToListAsync();
        }

        // GET: api/AddBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddBook>> GetAddBook(int id)
        {
          if (_context.AddBook == null)
          {
              return NotFound();
          }
            var addBook = await _context.AddBook.FindAsync(id);

            if (addBook == null)
            {
                return NotFound();
            }

            return addBook;
        }

        // PUT: api/AddBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddBook(int id, AddBook addBook)
        {
            if (id != addBook.Id)
            {
                return BadRequest();
            }

            _context.Entry(addBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AddBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AddBook>> PostAddBook([FromBody] AddBook addBook)
        {
            if (addBook == null)
                return BadRequest();

            if (await CheckNameExistAsync(addBook.Name))
                return BadRequest(new { Message = "Book Already Exist" });

            await _context.AddAsync(addBook);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Book Added Successfully!"
            });

        }

        private Task<bool> CheckNameExistAsync(string? name)
            => _context.AddBook.AnyAsync(x => x.Name == name);

        // DELETE: api/AddBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddBook(int id)
        {
            if (_context.AddBook == null)
            {
                return NotFound();
            }
            var addBook = await _context.AddBook.FindAsync(id);
            if (addBook == null)
            {
                return NotFound();
            }

            _context.AddBook.Remove(addBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddBookExists(int id)
        {
            return (_context.AddBook?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

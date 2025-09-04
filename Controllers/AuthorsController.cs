using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {

        private readonly BookStoreContext _context;

        public AuthorsController(BookStoreContext context)
        {
            _context = context;
        }

        //Get: api/authors
        [HttpGet]
        //Returns a list of AuthorDto objects wrapped in an HTTP response (ActionResult).
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors
            //Include tells EF Core to load related entities — in this case, each author's books.
            .Include(a => a.Books)
            //instead of returning full Author entities (with all database fields), we create a DTO (Data Transfer Object).
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                Biography = a.Biography,
                Nationality = a.Nationality,
                Books = a.Books.Select(b => new BookSummaryDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price
                }).ToList()
            })
            .ToListAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth,
                Nationality = author.Nationality,
                Books = author.Books.Select(b => new BookSummaryDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price
                }).ToList()

            };

            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
        {

            var author = new Author
            {
                Name = createAuthorDto.Name,
                Biography = createAuthorDto.Biography,
                DateOfBirth = createAuthorDto.DateOfBirth,
                Nationality = createAuthorDto.Nationality

            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth,
                Nationality = author.Nationality,
                Books = new List<BookSummaryDto>()
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
        {

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            author.Name = updateAuthorDto.Name;
            author.Biography = updateAuthorDto.Biography;
            author.DateOfBirth = updateAuthorDto.DateOfBirth;
            author.Nationality = updateAuthorDto.Nationality;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!AuthorExist(id))
                {
                    return NotFound();
                }

                throw;

            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();

        }


        private bool AuthorExist(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }

    }
}


//Notes: 
//ActionResult<T>:
//Means your endpoint returns data of type T (like a DTO) wrapped in an HTTP response.
//Useful for GET or POST where you’re returning something back (like a resource or list).
//IActionResult:
//Means you only care about the HTTP response (200, 204, 404, etc.), not returning data.
//Useful for PUT and DELETE, where the client doesn’t need the updated object back.


//Use ActionResult<T> when returning data.
//Use IActionResult when only sending a status (no body needed).
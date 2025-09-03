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


            return CreatedAtAction(nameof(GetAuthor), new{id = author.Id}, authorDto);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
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
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors
            .Include(a => a.Books)
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




    }
}

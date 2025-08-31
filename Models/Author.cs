using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Biography { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
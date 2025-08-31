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

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string Biography { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; } = string.Empty;
        

    }
}
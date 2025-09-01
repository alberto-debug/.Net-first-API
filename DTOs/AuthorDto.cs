using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAPI.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Biography { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? Nationality { get; set; }

        public List<BookSummaryDto> Books { get; set; } = new List<BookSummaryDto>();

    }

    public class CreateAuthorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Biography { get; set; } 
        public DateTime DateOfBirth{ get; set; }
        public string? Nationality{get;set;}


    }

    public class UpdateAuthorDto{
        public string Name{get;set;} = string.Empty;
        public string? Biography {get;set;}
        public DateTime DateOfBirth{get;set;}
        public string? Nationality{get;set;}
    }





}

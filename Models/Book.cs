using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    [Index(new string[] { nameof(Title), nameof(Author), nameof(Genre) }, IsUnique = true)]
    public class Book
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(100)]
        public string Genre { get; set; }

        [Required]
        public decimal Price { get; set; }

    }

}

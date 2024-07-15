using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    [Index(nameof(Title), nameof(Author), nameof(Genre), IsUnique = true)]
    [Index(nameof(Title))]
    [Index(nameof(Author))]
    public class Book
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} length should be between {2} and {1}")]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} length should be between {2} and {1}")]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("Author")]
        public string Author { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} length should be between {2} and {1}")]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "Only alphabets are allowed")]
        [DisplayName("Genre")]
        public string Genre { get; set; }


        [Range(0, 10000, ErrorMessage = "{0} should be between {2} and {1}")]
        [DisplayName("Price")]
        [DefaultValue(0)]
        public decimal Price { get; set; }

    }

}

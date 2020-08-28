using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shortener.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //[CheckUrlValid(ErrorMessage = "Please enter a valid Url")]
        [Url]
        public string LongUrl { get; set; }
        [Required]
        public string ShortUrl { get; set; }
        public string DateCreated { get; set; } = DateTime.Today.ToString("dd/MM/yyyy");
        public int UseCount { get; set; } = 0;
    }
}

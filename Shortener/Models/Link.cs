using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shortener.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        [Required]
        
        [Url]
        [Column(TypeName = "varchar(2000)")]
        public string LongUrl { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string ShortUrl { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string DateCreated { get; set; } = DateTime.Today.ToString("dd/MM/yyyy");

        public int UseCount { get; set; } = 0;
    }
}

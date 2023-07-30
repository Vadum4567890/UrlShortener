﻿using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
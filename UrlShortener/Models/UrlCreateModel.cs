using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlCreateModel
    {
        [Required]
        public string OriginalUrl { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}

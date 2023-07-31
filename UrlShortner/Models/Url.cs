using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OriginalUrl { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}

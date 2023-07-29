using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Models
{
    public class AlgorithmDescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

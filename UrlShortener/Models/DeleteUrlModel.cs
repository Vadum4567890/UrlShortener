using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class DeleteUrlModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
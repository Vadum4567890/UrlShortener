using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
    [Keyless]
    public class AboutModel
    {
        public string Description { get; set; }
    }
}

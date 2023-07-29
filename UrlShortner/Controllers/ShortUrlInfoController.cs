using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Interfaces;

namespace UrlShortner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlInfoController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public ShortUrlInfoController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetShortUrlInfo(int id)
        {
            var shortUrl = _urlService.GetUrlById(id);

            if (shortUrl != null)
            {
                return Ok(shortUrl);
            }

            return NotFound(new { message = "Скорочений URL не знайдено." });
        }
    }
}

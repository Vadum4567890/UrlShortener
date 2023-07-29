using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShorter.Interfaces;
using UrlShorter.ViewModel;

namespace UrlShorter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public ShortUrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        public IActionResult GetShortUrls()
        {
            var shortUrls = _urlService.GetAllUrls();
            return Ok(shortUrls);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddShortUrl([FromBody] ShortUrlViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUrl = _urlService.ShortenUrl(model.OriginalUrl, User.Identity.Name);
                return Ok(newUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteShortUrl(int id)
        {
            var username = User.Identity.Name;
            var isDeleted = _urlService.DeleteUrl(id, username);

            if (isDeleted)
            {
                return Ok(new { message = "Скорочений URL успішно видалено." });
            }

            return NotFound(new { message = "Скорочений URL не знайдено або у вас немає прав для видалення." });
        }
    }
}

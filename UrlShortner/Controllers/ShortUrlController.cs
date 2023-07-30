using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.ViewModels;
using UrlShortner.Interfaces;


namespace UrlShortner.Controllers
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
                var userId = User.Identity.Name; // Отримуємо ідентифікатор користувача з імені користувача з авторизованого запиту
                var newUrl = _urlService.ShortenUrl(model.OriginalUrl, userId);
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
            var userId = User.Identity.Name; // Отримуємо ідентифікатор користувача з імені користувача з авторизованого запиту
            var isDeleted = _urlService.DeleteUrl(id, userId);

            if (isDeleted)
            {
                return Ok(new { message = "Скорочений URL успішно видалено." });
            }

            return NotFound(new { message = "Скорочений URL не знайдено або у вас немає прав для видалення." });
        }
    }
}

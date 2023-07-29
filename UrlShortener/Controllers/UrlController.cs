using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using Microsoft.AspNetCore.Authorization;
using UrlShortener.Interfaces;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only authenticated users can access these endpoints
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllUrls()
        {
            var urls = _urlService.GetAllUrls();
            return Ok(urls);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetUrlById(int id)
        {
            var url = _urlService.GetUrlById(id);
            if (url == null)
            {
                return NotFound();
            }

            return Ok(url);
        }

        [HttpPost]
        public IActionResult ShortenUrl([FromBody] UrlCreateModel model)
        {
            try
            {
                var newUrl = _urlService.ShortenUrl(model.OriginalUrl, model.UserName);
                return CreatedAtAction(nameof(GetUrlById), new { id = newUrl.Id }, newUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteUrl(int id, [FromBody] DeleteUrlModel model)
        {
            if (_urlService.DeleteUrl(id, model.UserName))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
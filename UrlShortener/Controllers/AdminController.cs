using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")] // Only users with "Admin" role can access these endpoints
    public class AdminController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public AdminController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpDelete("delete-all")]
        public IActionResult DeleteAllUrls()
        {
            _urlService.DeleteAllUrls();

            return Ok(new { message = "All URLs deleted successfully." });
        }

        [HttpPost("add-url")]
        public IActionResult AddUrl([FromBody] UrlCreateModel model)
        {
            try
            {
                var newUrl = _urlService.ShortenUrl(model.OriginalUrl, "Admin");
                return CreatedAtAction(nameof(GetUrlById), new { id = newUrl.Id }, newUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-url/{id}")]
        public IActionResult GetUrlById(int id)
        {
            var url = _urlService.GetUrlById(id);
            if (url == null)
            {
                return NotFound(new { message = "URL not found." });
            }

            return Ok(url);
        }

        [HttpGet("get-all-urls")]
        public IActionResult GetAllUrls()
        {
            var urls = _urlService.GetAllUrls();
            return Ok(urls);
        }

        [HttpDelete("delete-url/{id}")]
        public IActionResult DeleteUrl(int id)
        {
            var url = _urlService.GetUrlById(id);
            if (url == null)
            {
                return NotFound(new { message = "URL not found." });
            }

            _urlService.DeleteUrl(id, "Admin");
            return Ok(new { message = "URL deleted successfully." });
        }
    }
}
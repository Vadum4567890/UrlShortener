using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AboutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: About
        [AllowAnonymous]
        public IActionResult About()
        {
            AboutModel about = GetAboutModelFromDatabase(); // Retrieve the AboutModel from the database
            return View(about);
        }

        // POST: About
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public IActionResult About(AboutModel about)
        {
            if (!ModelState.IsValid)
            {
                return View(about);
            }

            // Save or update the AboutModel in the database
            SaveOrUpdateAboutModelInDatabase(about);

            return RedirectToAction("About");
        }

        private AboutModel GetAboutModelFromDatabase()
        {
            // Your logic to retrieve the AboutModel from the database
            // For demonstration purposes, let's assume we return a default model
            return new AboutModel
            {
                Description = "Default description"
            };
        }

        private void SaveOrUpdateAboutModelInDatabase(AboutModel about)
        {
            // Your logic to save or update the AboutModel in the database
            // For demonstration purposes, we'll just replace the model in the list
            _context.AboutModel.RemoveRange(_context.AboutModel);
            _context.AboutModel.Add(about);
            _context.SaveChanges();
        }
    }

}

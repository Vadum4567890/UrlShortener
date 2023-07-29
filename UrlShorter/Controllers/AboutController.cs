using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UrlShorter.Data;
using UrlShorter.Models;

namespace UrlShorter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AboutController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var algorithmDescription = _dbContext.AlgorithmDescriptions.FirstOrDefault();

            // If the AlgorithmDescription is null, create a new instance with default values
            if (algorithmDescription == null)
            {
                algorithmDescription = new AlgorithmDescription
                {
                    Description = "Default Description"
                };
            }

            return View(algorithmDescription);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit()
        {
            var algorithmDescription = _dbContext.AlgorithmDescriptions.FirstOrDefault();
            return View(algorithmDescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string newDescription)
        {
            if (ModelState.IsValid)
            {
                var algorithmDescription = _dbContext.AlgorithmDescriptions.FirstOrDefault();
                if (algorithmDescription == null)
                {
                    algorithmDescription = new AlgorithmDescription
                    {
                        Description = newDescription
                    };
                    _dbContext.AlgorithmDescriptions.Add(algorithmDescription);
                }
                else
                {
                    algorithmDescription.Description = newDescription;
                    _dbContext.AlgorithmDescriptions.Update(algorithmDescription);
                }

                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

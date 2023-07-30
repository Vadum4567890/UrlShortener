using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UrlShortner.Data;
using UrlShortner.Models;

namespace UrlShortner.Controllers
{
    [Authorize(Roles = "admin")]
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
            //if (algorithmDescription == null)
            //{
            //    algorithmDescription = new AlgorithmDescription
            //    {
            //        Description = "Default Description"
            //    };
            //}

            return View(algorithmDescription);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit()
        {
            var algorithmDescription = _dbContext.AlgorithmDescriptions.FirstOrDefault();
            return View(algorithmDescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(AlgorithmDescription model)
        {
            if (ModelState.IsValid)
            {
                var algorithmDescription = _dbContext.AlgorithmDescriptions.FirstOrDefault();
                if (algorithmDescription == null)
                {
                    algorithmDescription = new AlgorithmDescription
                    {
                        Description = model.Description
                    };
                    _dbContext.AlgorithmDescriptions.Add(algorithmDescription);
                }
                else
                {
                    algorithmDescription.Description = model.Description;
                    _dbContext.AlgorithmDescriptions.Update(algorithmDescription);
                }

                _dbContext.SaveChanges();
                return RedirectToAction("About", "Home");
            }

            return View(model);
        }
    }
}

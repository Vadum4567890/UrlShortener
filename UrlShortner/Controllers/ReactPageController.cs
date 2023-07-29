using Microsoft.AspNetCore.Mvc;

namespace UrlShortner.Controllers
{
    public class ReactPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

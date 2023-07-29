using Microsoft.AspNetCore.Mvc;

namespace UrlShorter.Controllers
{
    public class ReactPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

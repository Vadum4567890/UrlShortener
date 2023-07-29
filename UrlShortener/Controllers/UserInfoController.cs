using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Забезпечуємо захист роуту, тільки автентифіковані користувачі можуть отримати інформацію про себе
    public class UserInfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            // Отримуємо ідентифікатор користувача з токену
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Отримуємо роль користувача з токену
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Тут можна виконати додаткові запити до бази даних для отримання інформації про користувача
            // Наприклад, звернення до бази даних для отримання даних про користувача зі збереженим userId.

            // Повертаємо інформацію про користувача у відповідь на запит
            return Ok(new { UserId = userId, Role = userRole });
        }
    }
}

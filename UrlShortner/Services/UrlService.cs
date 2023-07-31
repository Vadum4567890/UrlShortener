using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UrlShortner.Data;
using UrlShortner.Interfaces;
using UrlShortner.Models;

namespace UrlShortner.Services
{
    public class UrlService : IUrlService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public UrlService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public List<Url> GetAllUrls()
        {
            return _dbContext.Urls.ToList();
        }

        public Url GetUrlById(int id)
        {
            return _dbContext.Urls.FirstOrDefault(u => u.Id == id);
        }

        public Url ShortenUrl(string originalUrl, string userName)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be empty or contain only whitespaces.");
            }
            if (UrlExists(originalUrl))
            {
                throw new Exception("URL already exists.");
            }

            var shortUrl = GenerateShortUrl(originalUrl);
            // Перевірка, чи користувач аутентифікований
            string createdBy = string.IsNullOrWhiteSpace(userName) ? "Anonymous" : userName;

            var newUrl = new Url
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Urls.Add(newUrl);
            _dbContext.SaveChanges();

            return newUrl;
        }

        public async Task<bool> CanDeleteUrl(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return false;
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return isAdmin;
        }
        public async Task<bool> DeleteUrl(int Id, string UserName)
        {
            // видалення працює але фіча з передаванням користувача на клієнт ще не реалізована
            var urlToDelete = _dbContext.Urls.FirstOrDefault(u => u.Id == Id); 
            // var urlToDelete = _dbContext.Urls.FirstOrDefault(u => u.Id == Id && u.CreatedBy == UserName);
 
            //if (urlToDelete != null)
            //{
            //    bool isAdmin = await CanDeleteUrl(UserName);

            //    if (isAdmin || urlToDelete.CreatedBy == UserName)
            //    {
            //        _dbContext.Urls.Remove(urlToDelete);
            //        _dbContext.SaveChanges();
            //        return true;
            //    }
            //}
            if (urlToDelete != null)
            {
                _dbContext.Urls.Remove(urlToDelete);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

     

        private string GenerateShortUrl(string originalUrl)
        {
            // Задаємо регулярний вираз для пошуку частини перед доменом
            string pattern = @"^(?:https?:\/\/)?(?:[^@\n]+@)?(?:www\.)?([^:\/\n?]+)";
            Match match = Regex.Match(originalUrl, pattern);
            if (match.Success)
            {
                string shortUrl = match.Groups[1].Value;
                return shortUrl;
            }
            // Якщо не знайдено відповідності, повертаємо весь `originalUrl`
            return originalUrl;
        }


        private bool UrlExists(string originalUrl)
        {
            return _dbContext.Urls.Any(u => u.OriginalUrl == originalUrl);
        }
    }
}

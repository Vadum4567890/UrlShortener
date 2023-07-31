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

        public UrlService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public bool DeleteUrl(int Id, string UserName)
        {
            var urlToDelete = _dbContext.Urls.FirstOrDefault(u => u.Id == Id && u.CreatedBy == UserName);

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

            // Пошук відповідності регулярному виразу
            Match match = Regex.Match(originalUrl, pattern);

            // Якщо знайдено відповідність, повертаємо групу з результатом
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

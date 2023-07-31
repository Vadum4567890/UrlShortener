using System.Security.Cryptography;
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
            if (UrlExists(originalUrl))
            {
                throw new Exception("URL already exists.");
            }

            var shortUrl = GenerateShortUrl();

            var newUrl = new Url
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
                CreatedBy = userName,
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

     

        private string GenerateShortUrl()
        {
            byte[] randomBytes = new byte[6];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            string base64String = Convert.ToBase64String(randomBytes);

            base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return base64String;
        }


        private bool UrlExists(string originalUrl)
        {
            return _dbContext.Urls.Any(u => u.OriginalUrl == originalUrl);
        }
    }
}

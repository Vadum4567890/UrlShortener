using System.Security.Cryptography;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Services
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

        public Url ShortenUrl(string OriginalUrl, string UserName)
        {
            if (UrlExists(OriginalUrl))
            {
                throw new Exception("URL already exists.");
            }

            var ShortUrl = GenerateShortUrl();

            var newUrl = new Url
            {
                OriginalUrl = OriginalUrl,
                ShortUrl = ShortUrl,
                CreatedBy = UserName,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Urls.Add(newUrl);
            _dbContext.SaveChanges();

            return newUrl;
        }

        public bool DeleteUrl(int id, string username)
        {
            var urlToDelete = _dbContext.Urls.FirstOrDefault(u => u.Id == id && u.CreatedBy == username);

            if (urlToDelete != null)
            {
                _dbContext.Urls.Remove(urlToDelete);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool UrlExists(string originalUrl)
        {
            return _dbContext.Urls.Any(u => u.OriginalUrl == originalUrl);
        }

        private string GenerateShortUrl()
        {
            // Generate a random 6-byte array (48 bits)
            byte[] randomBytes = new byte[6];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the byte array to a Base64 string
            string base64String = Convert.ToBase64String(randomBytes);

            // Remove any characters that might not be URL-safe
            base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return base64String;
        }

        public void DeleteAllUrls()
        {
            var allUrls = _dbContext.Urls.ToList();
            _dbContext.Urls.RemoveRange(allUrls);
            _dbContext.SaveChanges();
        }
    }
}

using UrlShortener.Models;

namespace UrlShortener.Interfaces
{
    public interface IUrlService
    {
        List<Url> GetAllUrls();
        Url GetUrlById(int id);
        Url ShortenUrl(string OriginalUrl, string UserName);
        bool DeleteUrl(int Id, string UserName);
        bool UrlExists(string OriginalUrl);
        void DeleteAllUrls();
    }

}

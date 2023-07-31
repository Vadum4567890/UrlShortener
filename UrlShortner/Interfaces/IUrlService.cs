using UrlShortner.Models;

namespace UrlShortner.Interfaces
{
    public interface IUrlService
    {
        List<Url> GetAllUrls();
        Url GetUrlById(int Id);
        Url ShortenUrl(string OriginalUrl, string UserName);
        Task<bool> DeleteUrl(int Id, string UserName);
    }
}

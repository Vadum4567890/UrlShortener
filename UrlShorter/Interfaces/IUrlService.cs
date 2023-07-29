using UrlShorter.Models;

namespace UrlShorter.Interfaces
{
    public interface IUrlService
    {
        List<Url> GetAllUrls();
        Url GetUrlById(int Id);
        Url ShortenUrl(string OriginalUrl, string UserName);
        bool DeleteUrl(int Id, string UserName);
    }
}

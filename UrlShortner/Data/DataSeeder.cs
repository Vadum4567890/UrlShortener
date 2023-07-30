using Microsoft.EntityFrameworkCore;
using UrlShortner.Models;

namespace UrlShortner.Data
{
    public class DataSeeder
    {
        public static void SeedData(ApplicationDbContext dbContext)
        {
            // Check if the database is empty or needs seeding
            if (!dbContext.AlgorithmDescriptions.Any())
            {
                // Add initial data here
                var algorithmDescription = new Models.AlgorithmDescription
                {
                    Description = "Default Description"
                };

                dbContext.AlgorithmDescriptions.Add(algorithmDescription);
                dbContext.SaveChanges();
            }

            if (!dbContext.Urls.Any())
            {
                var url1 = new Url { CreatedBy = "meuf", CreatedDate = DateTime.Now, ShortUrl = "https://google.com", OriginalUrl = "https://google.com/maps" };
                var url2 = new Url { CreatedBy = "keuf", CreatedDate = DateTime.Now, ShortUrl = "https://youtube.com", OriginalUrl = "https://youtube.com/fdsdssn31341nsl" };
                dbContext.Urls.AddRange(url1,url2);
                dbContext.SaveChanges();
            }
        }
    }
}

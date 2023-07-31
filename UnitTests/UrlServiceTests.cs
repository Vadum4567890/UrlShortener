using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UrlShortner.Data;
using UrlShortner.Models;
using UrlShortner.Services;

namespace UnitTests
{
    [TestClass]
    public class UrlServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            // Set up an in-memory database for testing
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlShortnerDataBase")
                .Options;
        }

        [TestMethod]
        public void ShortenUrl_WithValidInput_ShouldAddNewUrl()
        {
            // Arrange
            var originalUrl = "http://www.example.com";
            var userName = "testuser";

            // Act
            using (var context = new ApplicationDbContext(_options))
            {
                var urlService = new UrlService(context, null);
                var newUrl = urlService.ShortenUrl(originalUrl, userName);

                // Assert
                Assert.IsNotNull(newUrl);
                Assert.AreEqual(originalUrl, newUrl.OriginalUrl);
                Assert.IsFalse(string.IsNullOrWhiteSpace(newUrl.ShortUrl));
                Assert.AreEqual(userName, newUrl.CreatedBy);
                Assert.AreEqual(DateTime.UtcNow.Date, newUrl.CreatedDate.Date);

                // Check if the URL was added to the database
                Assert.AreEqual(1, context.Urls.Count());
            }
        }

        [TestMethod]
        public async Task TestDeleteUrl()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlShortnerDataBase")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                // Create test data
                var url = new Url
                {
                    Id = 1,
                    OriginalUrl = "https://www.example.com",
                    ShortUrl = "example",
                    CreatedBy = "testuser",
                    CreatedDate = DateTime.UtcNow
                };
                dbContext.Urls.Add(url);
                dbContext.SaveChanges();
            }

            // Mock UserManager
            var user = new IdentityUser { UserName = "testuser" };
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.FindByNameAsync("testuser")).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.IsInRoleAsync(It.IsAny<IdentityUser>(), "Admin")).ReturnsAsync(false);

            using (var dbContext = new ApplicationDbContext(options))
            {
                // Create an instance of the UrlService using the in-memory DbContext and mocked UserManager
                var urlService = new UrlService(dbContext, mockUserManager.Object);

                // Act
                bool isDeleted = await urlService.DeleteUrl(1, "testuser");

                // Assert
                Assert.IsTrue(isDeleted);
            }
        }
    }
}
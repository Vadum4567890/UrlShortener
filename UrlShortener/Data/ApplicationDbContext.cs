using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<AboutModel> AboutModel { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AboutModel>().HasKey(a => a.Description);
            // Seed the admin user
            var adminRoleId = "admin-role-id"; // Replace this with the actual admin role ID
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            // Seed the admin user
            var adminEmail = "admin@example.com";
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            modelBuilder.Entity<IdentityUser>().HasData(adminUser);

            // Assign the admin role to the admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRoleId
            });
        }

        public IdentityUser GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                // Знайдено ідентифікатор користувача, можна виконати запит до бази даних, щоб отримати потрібного користувача
                return this.Users.FirstOrDefault(u => u.Id == userId);
            }

            // Якщо ідентифікатор користувача не знайдено або користувач не авторизований, поверніть null або зробіть інше відповідне дії
            return null;
        }
    }
}
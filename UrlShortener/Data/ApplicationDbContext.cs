using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<AboutModel> AboutModel { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
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
    }
}
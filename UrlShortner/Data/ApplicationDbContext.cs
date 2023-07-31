using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Models;

namespace UrlShortner.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<AlgorithmDescription> AlgorithmDescriptions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AlgorithmDescription>()
              .HasIndex(ad => ad.Description)
              .IsUnique();


            var adminRoleId = "AdminRoleId"; // Replace with a unique identifier for the admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "admin",
                NormalizedName = "ADMIN"
            });
        }

    }
}
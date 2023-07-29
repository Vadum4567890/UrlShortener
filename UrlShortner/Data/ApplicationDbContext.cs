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

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        //{

        //    if (!roleManager.RoleExistsAsync("Admin").Result)
        //    {
        //        var adminRole = new IdentityRole("Admin");
        //        roleManager.CreateAsync(adminRole).Wait();
        //    }

        //    if (!roleManager.RoleExistsAsync("User").Result)
        //    {
        //        var userRole = new IdentityRole("User");
        //        roleManager.CreateAsync(userRole).Wait();
        //    }

        //    CreateUserIfNotExists(app, env, userManager, "admin@example.com", "adminpassword", "Admin");
        //    CreateUserIfNotExists(app, env, userManager, "user@example.com", "userpassword", "User");

        //}

        //private void CreateUserIfNotExists(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, string email, string password, string role)
        //{
        //    var user = userManager.FindByEmailAsync(email).Result;
        //    if (user == null)
        //    {
        //        user = new ApplicationUser
        //        {
        //            UserName = email,
        //            Email = email
        //        };
        //        var result = userManager.CreateAsync(user, password).Result;
        //        if (result.Succeeded)
        //        {
        //            userManager.AddToRoleAsync(user, role).Wait();
        //        }
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AlgorithmDescription>()
              .HasIndex(ad => ad.Description)
              .IsUnique();
        }
    }
}
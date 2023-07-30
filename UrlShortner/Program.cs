using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;
using UrlShortner.Interfaces;
using UrlShortner.Models;
using Microsoft.Extensions.Options;
using UrlShortener.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", option => option.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = false;
})
.AddRoles<IdentityRole>() // Add this line to register the RoleManager service
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    // Налаштування таймауту сесії (наприклад, 20 хвилин)
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    // Налаштування cookie для сесії
    options.Cookie.HttpOnly = true;
});


using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await CreateAdminUser(scope.ServiceProvider, configuration);
}

async Task CreateAdminUser(IServiceProvider services, IConfiguration configuration)
{
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var adminEmail = configuration["AdminSettings:Email"];
    var adminPassword = configuration["AdminSettings:Password"];

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            var adminRole = await roleManager.FindByNameAsync("admin");
            if (adminRole != null)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
        }
        else
        {
            // Handle the error when creating the administrator
            // (result.Errors contains the list of errors)
        }
    }
}
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed the database with initial data
    DataSeeder.SeedData(dbContext);

    await CreateAdminUser(scope.ServiceProvider, configuration);
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

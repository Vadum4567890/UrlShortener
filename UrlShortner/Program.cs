using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;
using UrlShortner.Interfaces;
using UrlShortner.Services;


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
.AddRoles<IdentityRole>() 
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
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
            Email = adminEmail,
            EmailConfirmed = true
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
            throw new Exception("Nice Bro");
            
        }
    }
}

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DataSeeder.SeedData(dbContext);
    await CreateAdminUser(scope.ServiceProvider, configuration);
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Configure Cors Policy
app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

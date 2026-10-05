using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sami_Archive.Data;
using Sami_Archive.Models;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container

var connectionString = builder.Configuration.GetConnectionString("SamisArchiveConnection") ?? throw new InvalidOperationException("Connection string 'SamisArchiveConnection' not found.");
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<StoreDbContext>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookRepository, EFBookRepository>();
builder.Services.AddScoped<IGenreRepository, EFGenreRepository>();
builder.Services.AddScoped<IAuthorRepository, EFAuthorRepository>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password setttings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;


    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 99;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

    // Conffirmed account to login to
    options.SignIn.RequireConfirmedAccount = true;
    // Token lifespan: 1 day is default
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});


// -----> Looking for a standard MVC controller that does not exist
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//    options.SlidingExpiration = true;
//});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
}
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();


app.MapControllerRoute("book",
    "Books/Page{bookPage}",
    new { Controller = "BookController", action = "Get" });

app.MapControllerRoute("genre",
    "Genres/Page{genrePage}",
    new { Controller = "GenreController", action = "Get" });

app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();

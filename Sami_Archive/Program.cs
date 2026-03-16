using Microsoft.EntityFrameworkCore;
using Sami_Archive.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:SamisArchiveConnection"]);
});

builder.Services.AddScoped<IBookRepository, EFBookRepository>();
builder.Services.AddScoped<IGenreRepository, EFGenreRepository>();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllerRoute("book",
    "Books/Page{bookPage}",
    new { Controller = "BookController", action = "Get" });

app.MapControllerRoute("genre", 
    "Genres/Page{genrePage}", 
    new { Controller = "GenreController", action = "Get " });


app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();

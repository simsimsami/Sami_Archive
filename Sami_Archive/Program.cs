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

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllerRoute("pagination",
    "Books/Page{bookPage}",
    new { Controller = "Home", action = "Index" });

app.MapControllers();

SeedData.EnsurePopulated(app);

app.Run();

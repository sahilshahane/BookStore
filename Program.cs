using BookStore.Controllers;
using BookStore.Data;
using BookStore.Lib;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("BookStore")));
builder.Services.AddScoped<BookService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName.Equals("Development"))
{
    app.CreateDbIfNotExists();
}
else
{
    app.UseExceptionHandler("/Errors/InternalServer");
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/Errors/InternalServer");

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Books}/{action=Index}/{bookId?}");
app.MapControllerRoute(name: "PageNotFoundError", pattern: "{**slug}", defaults: new { controller = "Errors", action = "PageNotFound" });


app.Run();

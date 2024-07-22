using BookStore.Data;
using BookStore.Lib;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlite("Data Source=BookStore.db"));
builder.Services.AddScoped<BookService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName.Equals("Development"))
{
    app.CreateDbIfNotExists();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();

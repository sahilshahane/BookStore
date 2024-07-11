using BookStore.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSqlite<BookStoreContext>("Data Source=BookStore.db");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName.Equals("Development"))
{
    app.CreateDbIfNotExists();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{bookId?}"
);

//app.MapControllerRoute(
//    name: "List Books",
//    pattern: "{controller=Books}/{action=Index}"
//);

//app.MapControllerRoute(
//    name: "Edit Book",
//    pattern: "{controller=Books}/Edit/{bookId?}"
//);


app.Run();

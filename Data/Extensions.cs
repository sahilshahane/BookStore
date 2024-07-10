namespace BookStore.Data
{
    public static class Extensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<BookStoreContext>();

                    // TODO: remove EnsuredCreate() and
                    // find a way to check if db is created only through migrations
                    context.Database.EnsureCreated();


                    DbInitializer.Initialize(context);
                }
            }
        }
    }
}

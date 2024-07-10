using BookStore.Models;

namespace BookStore.Data
{
    public static class DbInitializer
    {

        // Seeds the database with dummy values
        public static void Initialize(BookStoreContext context)
        {

            //if any books are already present then do not reinitialize
            if (context.Books.Any()) return;

            var book1 = new Book();

            book1.Id = 1;
            book1.Title = "A";
            book1.Author = "SAHIL";
            book1.Price = 10;
            book1.Genre = "Action";

            var book2 = new Book();

            book2.Id = 2;
            book2.Title = "B";
            book2.Author = "JYOTI";
            book2.Price = 10000;
            book2.Genre = "Motivation";

            var book3 = new Book();

            book3.Id = 3;
            book3.Title = "C";
            book3.Author = "PRAVIN";
            book3.Price = 10000;
            book3.Genre = "Finance";

            var book4 = new Book();

            book4.Id = 4;
            book4.Title = "D";
            book4.Author = "SHRUTI";
            book4.Price = 5000;
            book4.Genre = "Self-Help";


            context.AddRange(new Book[] { book1, book2, book3, book4 });

            context.SaveChanges();

        }
    }
}

namespace BookStore.Services
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() : base("Book not found") { }
    }
    public class BookExistsException : Exception
    {
        public BookExistsException() : base("Book already exists") { }
    }
}

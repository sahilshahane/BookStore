using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class BookService
    {

        private readonly BookStoreContext _context;

        public BookService(BookStoreContext context)
        {
            this._context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.AsNoTracking().ToList();
        }

        public Book? GetById(int bookId)
        {
            return _context.Books.AsNoTracking().SingleOrDefault(p => p.Id == bookId);
        }

        public Book Create(Book newBook)
        {
            _context.Add(newBook);
            _context.SaveChanges();

            return newBook;
        }

        public Book FindBookByIdOrThrow(int bookId)
        {
            var book = _context.Books.Find(bookId);

            if (book == null)
            {
                throw new InvalidOperationException("Book not found");
            }

            return book;
        }

        public void UpdateBookId(int oldBookId, int newBookId)
        {
            var book = FindBookByIdOrThrow(oldBookId);

            book.Id = newBookId;

            _context.SaveChanges();
        }

        public void Update(Book updatedBook)
        {
            var book = FindBookByIdOrThrow(updatedBook.Id);

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;

            _context.SaveChanges();
        }

        public void Delete(int bookId)
        {
            var book = FindBookByIdOrThrow(bookId);

            _context.Remove(book);
            _context.SaveChanges();
        }



    }
}

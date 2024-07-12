using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using EntityFramework.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    internal static class BookServiceUtils
    {
        public static void SanitizeBookValues(Book book)
        {
            book.Title = book.Title.Trim();
            book.Author = book.Author.Trim();
            book.Genre = book.Genre.Trim();
        }
    }

    public class BookService
    {
        private readonly BookStoreContext _context;

        public BookService(BookStoreContext context)
        {
            _context = context;
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
            try
            {
                BookServiceUtils.SanitizeBookValues(newBook);


                _context.Add(newBook);
                _context.SaveChanges();
            }
            catch (UniqueConstraintException ex)
            {
                throw new BookExistsException();
            }



            return newBook;
        }

        public Book FindBookByIdOrThrow(int bookId)
        {
            var book = _context.Books.Find(bookId);

            if (book == null)
            {
                throw new BookNotFoundException();
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
            try
            {
                var book = FindBookByIdOrThrow(updatedBook.Id);

                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Genre = updatedBook.Genre;

                BookServiceUtils.SanitizeBookValues(book);

                _context.SaveChanges();
            }
            catch (UniqueConstraintException ex)
            {
                throw new BookExistsException();
            }
        }

        public void Delete(int bookId)
        {
            var book = FindBookByIdOrThrow(bookId);

            _context.Remove(book);
            _context.SaveChanges();
        }
    }

}

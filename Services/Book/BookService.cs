using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using EntityFramework.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookStore.Services
{
    internal static class BookServiceUtils
    {
        public static void SanitizeBookValues(Book book)
        {
            book.Title = Regex.Replace(book.Title.Trim(), @"\s+", " ");
            book.Author = Regex.Replace(book.Author.Trim(), @"\s+", " ");
            book.Genre = Regex.Replace(book.Genre.Trim(), @"\s+", " ");
        }
    }


    public class BookService
    {
        private readonly BookStoreContext _context;

        //private readonly ILogger _logger;

        public BookService(BookStoreContext context, ILoggerFactory logger)
        {
            _context = context;
            //_logger = logger.CreateLogger("BookStore.Services.BookService");
        }

        public enum LookupDirection
        {
            forward,
            backward
        }

        async public Task<List<Book>> ListAsync(int limit, int? cursorId, string? search, LookupDirection direction = LookupDirection.forward, int skip = 0)
        {
            if (limit <= 0) return [];

            var query = _context.Books.AsNoTracking().AsQueryable();

            switch (direction)
            {
                case LookupDirection.forward:
                    {
                        query = query.OrderBy(r => r.Id);

                        if (cursorId.HasValue) query = query.Where(r => r.Id >= cursorId);
                    }
                    break;
                case LookupDirection.backward:
                    {
                        query = query.OrderByDescending(r => r.Id);

                        if (cursorId.HasValue) query = query.Where(r => r.Id <= cursorId);
                    }
                    break;
            }

            if (search?.Length > 0)
            {
                var lowerCaseSearch = search.ToLower();

                query = query.Where(r => r.Title.ToLower().Contains(lowerCaseSearch) || r.Author.ToLower().Contains(lowerCaseSearch));
            }

            return await query.Skip(skip).Take(limit).ToListAsync();

        }

        async public Task<Book?> GetByIdAsync(int bookId)
        {
            return await _context.Books.AsNoTracking().SingleOrDefaultAsync(p => p.Id == bookId);
        }

        async public Task<Book> CreateAsync(Book newBook)
        {
            try
            {
                BookServiceUtils.SanitizeBookValues(newBook);


                await _context.AddAsync(newBook);
                await _context.SaveChangesAsync();
            }
            catch (UniqueConstraintException ex)
            {
                throw new BookExistsException();
            }

            return newBook;
        }

        async public Task<Book> FindBookByIdOrThrowAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                throw new BookNotFoundException();
            }

            return book;
        }

        async public Task UpdateAsync(Book updatedBook)
        {

            try
            {
                var book = await FindBookByIdOrThrowAsync(updatedBook.Id);

                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Genre = updatedBook.Genre;
                book.Price = updatedBook.Price;

                BookServiceUtils.SanitizeBookValues(book);

                await _context.SaveChangesAsync();
            }
            catch (UniqueConstraintException ex)
            {
                throw new BookExistsException();
            }


        }

        async public Task DeleteAsync(int bookId)
        {
            var book = await FindBookByIdOrThrowAsync(bookId);

            _context.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

}

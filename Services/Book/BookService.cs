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

        public BookService(BookStoreContext context)
        {
            _context = context;
        }



        public enum LookupDirection
        {
            forward,
            backward
        }

        public List<Book> List(int limit, int? cursorId, string? search, LookupDirection direction = LookupDirection.forward, int skip = 0)
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

            return query.Skip(skip).Take(limit).ToList();
        }

        public Book? GetById(int bookId)
        {
            return _context.Books.AsNoTracking().SingleOrDefault(p => p.Id == bookId);
        }


        public Book? GetBookByCursor(int cursorId, int skip, LookupDirection direction = LookupDirection.forward)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();

            switch (direction)
            {
                case LookupDirection.forward:
                    {
                        query = query.OrderBy(r => r.Id).Where(r => r.Id >= cursorId);
                    }
                    break;
                case LookupDirection.backward:
                    {
                        query = query.OrderByDescending(r => r.Id).Where(r => r.Id <= cursorId);
                    }
                    break;

                default: throw new NotImplementedException();
            }

            var book = query.Skip(skip).Take(1).FirstOrDefault();

            return book;
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

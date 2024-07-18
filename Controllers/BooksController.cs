using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookStore.Controllers
{


    [Route("{controller=Books}/{action=Index}/{bookId?}")]
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookStoreContext context)
        {
            this._bookService = new BookService(context);
        }



        [HttpGet]
        public IActionResult Index(BookListPaginationOptionsModel options)
        {
            const int MAX_LIMIT = 20;
            const int DEFAULT_LIMIT = 5;

            int limit = options.Limit.HasValue ? int.Max(int.Min(options.Limit.Value, MAX_LIMIT), 0) : DEFAULT_LIMIT;

            // limit + 1 is done to find if next-page exists
            var books = _bookService.List(limit + 1, options.CursorId, options.Search);

            var bookList = new BookListModel()
            {
                Books = [],
                Search = options.Search,
                Limit = limit,
                CursorId = options.CursorId,
            };

            if (books == null)
            {
                return View(bookList);
            }

            var bookCount = books.Count();

            // if this block executed then it indicates that next-page exists
            if (bookCount == limit + 1)
            {
                var lastBook = books.Last();
                bookList.NextCursorId = lastBook.Id;
                books.Remove(lastBook);
            }

            bookList.Books = books;

            if (options.CursorId.HasValue)
            {
                var prevCursorBook = _bookService.List(limit: 1, skip: limit, cursorId: options.CursorId.Value, search: options.Search, direction: BookService.LookupDirection.backward).LastOrDefault();

                if (prevCursorBook != null) bookList.PreviousCursorId = prevCursorBook.Id;
            }


            return View(bookList);
        }

        [HttpGet]
        public IActionResult Details(int bookId)
        {
            var book = _bookService.GetById(bookId);

            if (book == null) return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _bookService.Create(book);
            }
            catch (BookExistsException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book already exists" });
            }


            return RedirectToAction(nameof(Details), routeValues: new { bookId = book.Id });
        }


        [HttpGet]
        public IActionResult Edit(int? bookId)
        {
            try
            {
                if (!bookId.HasValue) throw new BookNotFoundException();


                var book = _bookService.GetById(bookId.Value);


                if (book == null) throw new BookNotFoundException();


                return View(book);
            }
            catch (BookNotFoundException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });
            }
        }



        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _bookService.Update(book);
            }
            catch (BookNotFoundException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });
            }
            catch (BookExistsException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book already exists" });
            }

            return RedirectToAction(nameof(Details), routeValues: new { bookId = book.Id });
        }


        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? bookId)
        {
            try
            {
                var book = _bookService.GetById(bookId.HasValue ? bookId.Value : 0);

                if (book == null) throw new BookNotFoundException();

                _bookService.Delete(book.Id);
            }
            catch (BookNotFoundException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });
            }

            return RedirectToAction(nameof(Delete));
        }
    }
}

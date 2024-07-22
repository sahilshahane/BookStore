using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookStore.Controllers
{


    [Route("{controller=Books}/{action=Index}/{bookId?}")]
    public class BooksController : Controller
    {
        private readonly BookService _bookService;
        private readonly ILogger _logger;

        public BooksController(BookService bookService, ILoggerFactory logger)
        {
            this._logger = logger.CreateLogger("BookStore.Controllers.BooksController");
            this._bookService = bookService;
        }


        [HttpGet]
        async public Task<IActionResult> Index(BookListPaginationOptionsModel options)
        {
            const int MAX_LIMIT = 20;
            const int DEFAULT_LIMIT = 5;

            int limit = options.Limit.HasValue ? int.Max(int.Min(options.Limit.Value, MAX_LIMIT), 0) : DEFAULT_LIMIT;

            // limit + 1 is done to find if next-page exists
            var books = await _bookService.ListAsync(limit + 1, options.CursorId, options.Search);

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
                var prevCursorBook = (await _bookService.ListAsync(limit: 1, skip: limit, cursorId: options.CursorId.Value, search: options.Search, direction: BookService.LookupDirection.backward)).LastOrDefault();

                if (prevCursorBook != null) bookList.PreviousCursorId = prevCursorBook.Id;
            }


            return View(bookList);
        }

        [HttpGet]
        async public Task<IActionResult> Details(int bookId)
        {
            var book = await _bookService.GetByIdAsync(bookId);

            if (book == null) return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        async public Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _bookService.CreateAsync(book);
            }
            catch (BookExistsException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book already exists" });
            }


            return RedirectToAction(nameof(Details), routeValues: new { bookId = book.Id });
        }


        [HttpGet]
        async public Task<IActionResult> Edit(int? bookId)
        {
            try
            {
                if (!bookId.HasValue) throw new BookNotFoundException();


                var book = await _bookService.GetByIdAsync(bookId.Value);


                if (book == null) throw new BookNotFoundException();


                return View(book);
            }
            catch (BookNotFoundException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });
            }
        }



        [HttpPost]
        async public Task<IActionResult> Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _bookService.UpdateAsync(book);
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
        async public Task<IActionResult> Delete(int? bookId)
        {
            try
            {
                var book = await _bookService.GetByIdAsync(bookId.HasValue ? bookId.Value : 0);

                if (book == null) throw new BookNotFoundException();

                await _bookService.DeleteAsync(book.Id);
            }
            catch (BookNotFoundException)
            {
                return View("Views/Shared/Error.cshtml", new ErrorViewModel() { Message = "Book not found" });
            }

            return RedirectToAction(nameof(Delete));
        }

    }
}

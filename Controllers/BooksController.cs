using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        public IActionResult Index(BookListPaginationOptions options)
        {
            return View(_bookService.List(options));
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

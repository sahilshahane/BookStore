using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookStoreContext context)
        {
            this._bookService = new BookService(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_bookService.GetAll());
        }

        [HttpGet]
        public IActionResult Details(int bookId)
        {
            return View(_bookService.GetById(bookId));
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public string Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return "invalid values provided";
            }


            _bookService.Create(book);

            return "ok";
        }


        [HttpGet]
        public IActionResult Edit(int? bookId)
        {
            var book = _bookService.GetById(bookId.HasValue ? bookId.Value : 0);
            return View(book);

        }



        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), routeValues: new { bookId = book.Id });
            }


            _bookService.Update(book);

            return RedirectToAction(nameof(Details), routeValues: new { bookId = book.Id });
        }


        [HttpGet]
        public IActionResult Delete()
        {
            //var book = _bookService.GetById(bookId.HasValue ? bookId.Value : 0);


            //if (book == null)
            //{
            //    return RedirectToAction(nameof(Error), new ErrorViewModel() { Message = "Book not found" });
            //}

            //_bookService.Delete(book.Id);

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? bookId)
        {
            var book = _bookService.GetById(bookId.HasValue ? bookId.Value : 0);


            if (book == null)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel() { Message = "Book not found" });
            }

            _bookService.Delete(book.Id);

            return RedirectToAction(nameof(Delete));
        }

        [HttpGet]
        public IActionResult Error(ErrorViewModel error)
        {
            return View(error);
        }
    }
}

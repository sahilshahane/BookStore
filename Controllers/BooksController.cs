using BookStore.Data;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

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
        public string Create()
        {
            return "ok";
        }

        [HttpPost]
        public string Create(Book book)
        {
            return "ok";
        }


        [HttpGet]
        public string Edit()
        {
            return "ok";
        }

        [HttpPost]
        public string Edit(Book book)
        {
            return "ok";
        }

        [HttpGet]
        public string Delete()
        {
            return "ok";
        }

        [HttpPost]
        public string Delete(int bookId)
        {
            return "ok";
        }

    }
}

using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult InternalServer()
        {
            return View();
        }
    }
}

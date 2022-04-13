using Microsoft.AspNetCore.Mvc;

namespace TestCase1.Controllers
{
    public class IndexController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
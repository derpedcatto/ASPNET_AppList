using classwork1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace classwork1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Basics()
        {
            Models.BasicsModel model = new()
            {
                Moment = DateTime.Now,
            };
            return View(model);
        }

        public IActionResult Razor()
        {
            Models.RazorModel model = new()
            {
                IntValue = 10,
                BoolValue = true,
                ListValue = new List<string> { "String 1", "String 2", "String 3", "String 4" }
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
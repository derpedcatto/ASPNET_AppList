using classwork4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace classwork4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Sessions(string? userstring)
        {
            if (userstring != null)
            {
                HttpContext.Session.SetString("StoredString", userstring);
            }

            if (HttpContext.Session.Keys.Contains("StoredString"))
            {
                ViewData["StoredString"] = HttpContext.Session.GetString("StoredString");
            }
            else
            {
                ViewData["StoredString"] = "У сесії немає збережених даних.";
            }

            if (HttpContext.Session.Keys.Contains("Form2String"))
            {
                ViewData["Form2String"] = HttpContext.Session.GetString("Form2String");
            }
            else
            {
                ViewData["Form2String"] = "У сесії немає збережених даних.";
            }
            return View();
        }

        public RedirectToActionResult SessionsForm(string? userstring)
        {
            // Цей метод приймає дані від другої форми і надсилає Redirect
            // Але для того щоб дані "userstring" були доступні після перезапиту,
            // їх необхідно зберегти у сесії

            HttpContext.Session.SetString("Form2String", userstring);
            return RedirectToAction("Sessions");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
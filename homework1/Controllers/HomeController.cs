using homework1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace homework1.Controllers
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

        public IActionResult Products()
        {
            ProductModel model = new()
            {
                Products = new()
            };
            model.Products.Add(new Product()
            {
                Name = "Red Bicycle",
                Price = 200,
                Image = "~/img/red_bycicle.jpg",
                Description = "A high-quality bicycle for everyday use"
            });
            model.Products.Add(new Product()
            {
                Name = "Leather Wallet",
                Price = 50,
                Image = "~/img/leather_wallet.jpg",
                Description = "A stylish and practical wallet made of genuine leather"
            });
            model.Products.Add(new Product()
            {
                Name = "Bluetooth Speaker",
                Price = 80,
                Image = "~/img/bluetooth_speaker.jpg",
                Description = "A powerful portable speaker with wireless Bluetooth connectivity"
            });
            model.Products.Add(new Product()
            {
                Name = "Smartphone Tripod",
                Price = 30,
                Image = "~/img/smartphone_tripod.jpg",
                Description = "A compact and lightweight tripod for smartphones"
            });
            model.Products.Add(new Product()
            {
                Name = "Wireless Mouse",
                Price = 40,
                Image = "~/img/wireless_mouse.jpg",
                Description = "A comfortable and responsive wireless mouse for your computer"
            });

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using DemoCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorksLT2022Context _db;

        public HomeController(ILogger<HomeController> logger, AdventureWorksLT2022Context db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult Privacy()
        {
            var products = _db.Product.FirstOrDefault();
            ViewBag.Product = products;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

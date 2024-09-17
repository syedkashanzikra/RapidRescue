using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Models;

namespace RapidRescue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


          [Route("/")]
          [Route("/home")]  
          public IActionResult Home() { 
           
            return View();
           }


    
        [Route("/contact")]
        public IActionResult Contact()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Contact", "")
    };
            return View(breadcrumbs);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

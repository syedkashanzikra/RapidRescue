using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RapidRescueContext _context; 

        public HomeController(ILogger<HomeController> logger, RapidRescueContext context)
        {
            _logger = logger;
            _context = context; 
        }

        // Home page
        [Route("/")]
        [Route("/home")]
        public IActionResult Home()
        {
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

            ViewBag.Breadcrumbs = breadcrumbs;

            return View();
        }



        [HttpPost]
        [Route("/contact")]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {

                _context.Contact.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Your message has been submitted successfully!";
                return RedirectToAction("Contact"); 
            }

            return View(model); 
        }




        
        [Route("/services")]
        public IActionResult Services()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/ambulance-car")]
        public IActionResult AmbulanceCar()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("Ambulance Car", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/medical-flight-services")]
        public IActionResult MedicalFlightServices()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("Medical Flight Services", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/medical-escort")]
        public IActionResult MedicalEscort()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("Medical Escort", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/private-air-ambulance")]
        public IActionResult PrivateAirAmbulance()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("Private Air Ambulance", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/advanced-life-support")]
        public IActionResult AdvancedLifeSupport()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("Advanced Life Support", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/general-services")]
        public IActionResult GeneralServices()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Services", Url.Action("Services", "Home")),
                new Tuple<string, string>("General Services", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/about")]
        public IActionResult About()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("About", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/team")]
        public IActionResult Team()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Team", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/testimonials")]
        public IActionResult Testimonials()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Testimonials", "")
            };
            return View(breadcrumbs);
        }

        
        [Route("/faq")]
        public IActionResult Faq()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("FAQ", "")
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

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace RapidRescue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RapidRescueContext _context; // Add the DbContext

        public HomeController(ILogger<HomeController> logger, RapidRescueContext context)
        {
            _logger = logger;
            _context = context; // Initialize the context
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
                return RedirectToAction("Contact"); // Redirect to avoid form re-submission
            }

            return View(model); // Return view with validation errors
        }




        // Services page (View All Services)
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

        // Ambulance Car Service
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

        // Medical Flight Services
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

        // Medical Escort Service
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

        // Private Air Ambulance Service
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

        // Advanced Life Support Service
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

        // General Services
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

        // New Route: About Our Company
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

        // New Route: Team
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

        // New Route: Testimonials
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

        // New Route: FAQ
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

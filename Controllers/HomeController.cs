using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;
using RapidRescue.Helpers;
using RapidRescue.Hubs;
using RapidRescue.Models;

namespace RapidRescue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RapidRescueContext _context;
        private readonly IHubContext<DriverLocationHub> _hubContext;
        private readonly NotificationHelper _notificationHelper; // Use NotificationHelper instead of Notification

        public HomeController(ILogger<HomeController> logger, RapidRescueContext context, IHubContext<DriverLocationHub> hubContext, NotificationHelper notificationHelper) // Updated to inject NotificationHelper
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
            _notificationHelper = notificationHelper; // Updated to assign NotificationHelper
        }


        // Home page
        [Route("/")]
        [Route("/home")]
        public IActionResult Home()
        {
            return View();
        }





        [HttpPost]
        [Route("/request-ambulance")]
        public async Task<IActionResult> RequestAmbulance([FromBody] Request incomingRequest)
        {
            // Check if the latitude and longitude are properly received
            Console.WriteLine($"Received latitude: {incomingRequest.PatientLatitude}, longitude: {incomingRequest.PatientLongitude}");

            // Validate the latitude and longitude
            if (incomingRequest.PatientLatitude == 0.0 || incomingRequest.PatientLongitude == 0.0)
            {
                Console.WriteLine("Error: Invalid latitude or longitude received.");
                return BadRequest("Invalid latitude or longitude received.");
            }

            // Fetch active drivers who are not currently handling a request (status is not "Dropped the Patient")
            var activeDrivers = _context.DriverInfo
                .Where(d => d.IsActive)
                .Where(d => !_context.Requests.Any(r => r.DriverId == d.DriverId && r.DriverStatus != "Dropped the Patient"))
                .ToList();

            if (activeDrivers.Count == 0)
            {
                Console.WriteLine("Error: No free drivers available at the moment.");
                return NotFound("No free drivers available at the moment. Please try again later.");
            }

            // Find the nearest driver
            var nearestDriver = activeDrivers
                .Select(driver => new
                {
                    Driver = driver,
                    Distance = GetDistance(incomingRequest.PatientLatitude, incomingRequest.PatientLongitude, driver.Latitude.Value, driver.Longitude.Value)
                })
                .OrderBy(d => d.Distance)
                .First().Driver;

            // Create a new request entry in the database
            var newRequest = new Request
            {
                DriverId = nearestDriver.DriverId,
                PatientLatitude = incomingRequest.PatientLatitude,  // Save exact latitude received from the frontend
                PatientLongitude = incomingRequest.PatientLongitude, // Save exact longitude received from the frontend
                RequestedAt = DateTime.UtcNow,
                DriverStatus = "Going to Patient"  // Initial status when the request is created
            };

            // Save the request to the database
            _context.Requests.Add(newRequest);
            await _context.SaveChangesAsync();

            // Get driver info from the user table based on the driver user_id
            var driverUser = await _context.Users.FirstOrDefaultAsync(u => u.User_id == nearestDriver.User_id);

            string message = $"Driver {driverUser.FirstName} {driverUser.LastName} has been assigned.";
            await _notificationHelper.CreateNotification("Driver Assignment", message); // Updated from _helpers to _notificationHelper

            // Broadcast the location update via SignalR
            await _hubContext.Clients.All.SendAsync("AssignDriver", nearestDriver.DriverId, incomingRequest.PatientLatitude, incomingRequest.PatientLongitude);

            return Ok(nearestDriver);
        }



        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Earth radius in kilometers
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Distance in kilometers

            // Check if the distance is 0 (or close to 0) and handle accordingly
            if (distance < 0.01) // For example, 10 meters
            {
                
                Console.WriteLine("Driver is already at the patient's location.");
            }

            return distance;
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
                return RedirectToAction("Home"); 
            }
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Contact", "")
            };

            ViewBag.Breadcrumbs = breadcrumbs;

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
            ViewBag.breadcrumbs = breadcrumbs;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

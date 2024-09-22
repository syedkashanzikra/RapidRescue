using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RapidRescue.Context;
using RapidRescue.Helpers;
using RapidRescue.Hubs;
using RapidRescue.Models;
using RapidRescue.ViewModels;
using static System.Net.WebRequestMethods;


namespace RapidRescue.Controllers
{
    public class DriverController : Controller
    {
        private readonly RapidRescueContext _context;
        private readonly IHubContext<DriverLocationHub> _hubContext;
        private readonly NotificationHelper _notificationHelper;
        public DriverController(RapidRescueContext context, IHubContext<DriverLocationHub> hubContext, NotificationHelper notificationHelper)
        {
            _context = context;
            _hubContext = hubContext;
            _notificationHelper = notificationHelper;
        }

        [Route("/get-drivers")]
        public IActionResult GetDrivers()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Drivers", "")
    };
            var drivers = _context.Users.Where(u => u.Role_Id == 3).ToList();

            var model = new DriverViewModel
            {
                Breadcrumbs = breadcrumbs,
                Drivers = drivers
            };

            return View(model);
        }

        [Route("/create-driver")]
        [HttpGet]
        public IActionResult CreateDriver()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Drivers", Url.Action("GetDrivers", "Drivers")),
        new Tuple<string, string>("Create Driver", "")
    };

            ViewBag.Breadcrumbs = breadcrumbs;

            return View();
        }

        [Route("/create-driver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDriver(CreateDriverViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Drivers", Url.Action("GetDrivers", "Drivers")),
        new Tuple<string, string>("Create Driver", "")
    };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View(model);
            }

            var passwordHasher = new PasswordHasher<Users>();

            var user = new Users
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = passwordHasher.HashPassword(null, model.Password),
                Role_Id = 3,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RememberToken="Added By Admin"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            var driverInfo = new DriverInfo
            {
                User_id = user.User_id,
                PhoneNumber = model.PhoneNumber,
                LicenseNumber = model.LicenseNumber,
                LicenseExpiryDate = model.LicenseExpiryDate,
                Address = model.Address,
                VehicleAssigned = model.VehicleAssigned,
                DateOfHire = model.DateOfHire,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.DriverInfo.Add(driverInfo);
            _context.SaveChanges();

            TempData["Message"] = "Driver added successfully.";
            return RedirectToAction("GetDrivers");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDriver(int userId)
        {
     
            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user == null)
            {
                
                TempData["Error"] = "User not found.";
                return RedirectToAction("GetDrivers");
            }

            
            var driverInfo = _context.DriverInfo.Where(d => d.User_id == userId).ToList();

            
            if (driverInfo.Any())
            {
                _context.DriverInfo.RemoveRange(driverInfo);
            }

            
            _context.Users.Remove(user);

            _context.SaveChanges();

            TempData["Message"] = "User and driver information deleted successfully.";

            return RedirectToAction("GetDrivers");
        }

        
        [HttpGet]
        [Route("/edit-driver/{id}")]
        public IActionResult EditDriver(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.User_id == id);
            var driverInfo = _context.DriverInfo.FirstOrDefault(d => d.User_id == id);

            if (user == null || driverInfo == null)
            {
                return NotFound();
            }

            var model = new EditDriverViewModel
            {
                User_id = user.User_id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = driverInfo.PhoneNumber,
                LicenseNumber = driverInfo.LicenseNumber,
                LicenseExpiryDate = driverInfo.LicenseExpiryDate,
                Address = driverInfo.Address,
                VehicleAssigned = driverInfo.VehicleAssigned,
                DateOfHire = driverInfo.DateOfHire,
                DriverInfo_id = driverInfo.DriverId
            };

            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Drivers", Url.Action("GetDrivers", "Drivers")),
        new Tuple<string, string>("Edit Driver", "")
    };

            ViewBag.Breadcrumbs = breadcrumbs;

            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/edit-driver/{id}")]
        public IActionResult EditDriver(EditDriverViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = new List<Tuple<string, string>>()

    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Drivers", Url.Action("GetDrivers", "Drivers")),
        new Tuple<string, string>("Edit Driver", "")
    };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.User_id == model.User_id);
            var driverInfo = _context.DriverInfo.FirstOrDefault(d => d.DriverId == model.DriverInfo_id);

            if (user == null || driverInfo == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<Users>();
                user.Password = passwordHasher.HashPassword(null, model.Password);
            }

            driverInfo.PhoneNumber = model.PhoneNumber;
            driverInfo.LicenseNumber = model.LicenseNumber;
            driverInfo.LicenseExpiryDate = model.LicenseExpiryDate;
            driverInfo.Address = model.Address;
            driverInfo.VehicleAssigned = model.VehicleAssigned;
            driverInfo.DateOfHire = model.DateOfHire;
            driverInfo.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            TempData["Message"] = "Driver details updated successfully.";
            return RedirectToAction("GetDrivers");

        }

        [HttpGet]
        public IActionResult UpdateStatus(int userId)
        {
            // Fetch the driver info based on User_id passed from the URL
            var driver = _context.DriverInfo.SingleOrDefault(d => d.User_id == userId);

         

            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Driver", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Update Status", "")
    };

            ViewBag.Breadcrumbs = breadcrumbs;
            ViewBag.Driver = driver;
            ViewBag.IsActive = driver.IsActive;
            return View(driver);
        }


        [HttpPost]
        public async Task<IActionResult> ActivateDriver(int userId)
        {
            var driver = _context.DriverInfo.SingleOrDefault(d => d.User_id == userId);

            if (driver != null)
            {
                driver.IsActive = true;  // Set driver as active
                driver.UpdatedAt = DateTime.UtcNow;  // Update timestamp

                // Start real-time location tracking
                await StartRealTimeLocationTracking(driver.DriverId);

                _context.SaveChanges();  // Save changes to the database
            }

            return RedirectToAction("UpdateStatus", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateDriver(int userId)
        {
            var driver = _context.DriverInfo.SingleOrDefault(d => d.User_id == userId);

            if (driver != null)
            {
                driver.IsActive = false;  // Set driver as inactive
                driver.UpdatedAt = DateTime.UtcNow;  // Update timestamp

                // Stop real-time location tracking
                await StopRealTimeLocationTracking(driver.DriverId);
               
                _context.SaveChanges();  // Save changes to the database
            }

            return RedirectToAction("UpdateStatus", new { userId });
        }

        private async Task StartRealTimeLocationTracking(int driverId)
        {
            await _hubContext.Clients.All.SendAsync("StartLocationTracking", driverId);
        }

        private async Task StopRealTimeLocationTracking(int driverId)
        {
            await _hubContext.Clients.All.SendAsync("StopLocationTracking", driverId);
        }



        // View that lists the driver's requests
        [HttpGet]
        public IActionResult DriverRequests(int userId)
        {
            // Fetch the driver's information based on the userId
            var driver = _context.DriverInfo.FirstOrDefault(d => d.User_id == userId);

            if (driver == null)
            {
                return NotFound("Driver not found");
            }

            // Fetch all requests associated with the driver
            var requests = _context.Requests.Where(r => r.DriverId == driver.DriverId).ToList();

            var breadcrumbs = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("Home", Url.Action("Home", "Home")),
            new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
            new Tuple<string, string>("Driver", Url.Action("GetDrivers", "Driver")),
            new Tuple<string, string>("Requests", "")
        };

            ViewBag.Breadcrumbs = breadcrumbs;

            // Return the view with the list of requests
            return View(requests);
        }

        // POST action to update the request status
        [HttpPost]
        public async Task<IActionResult> UpdateRequestStatus(int requestId, string status)
        {
            // Find the request in the database
            var request = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
            var userId = HttpContext.Session.GetInt32("user_id");


            if (request == null)
            {
                return NotFound("Request not found");
            }

            // Update the driver status
            request.DriverStatus = status;

            // Save changes to the database
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();


            // Redirect back to the driver's requests page
            return RedirectToAction("DriverRequests", new { userId =userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            var userId = HttpContext.Session.GetInt32("user_id");

            if (request == null)
            {
                return NotFound("Request not found.");
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            // Redirect back to the driver's requests list after deletion
            return RedirectToAction("DriverRequests", new { userId =userId });
        }










    }
}

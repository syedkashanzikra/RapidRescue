using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;
using RapidRescue.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RapidRescue.Controllers
{
    public class DriverController : Controller
    {
        private readonly RapidRescueContext _context;

        public DriverController(RapidRescueContext context) 
        {
            _context = context;
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
                IsActive = true,
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
                IsActive = true,
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

        [Route("/edit-driver/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

      
    }
}

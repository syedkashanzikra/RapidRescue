using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;
using RapidRescue.ViewModels;

namespace RapidRescue.Controllers
{
    public class EMTController : Controller
    {
        private readonly RapidRescueContext _context;
        private readonly PasswordHasher<Users> _passwordHasher = new PasswordHasher<Users>();

        public EMTController(RapidRescueContext context)
        {
            _context = context;
        }
        [Route("/get-emts")]
        public ActionResult GetEMTs()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Emt", "")
    };
            var emt = _context.Users.Where(u => u.Role_Id == 4).ToList();

            // Create a ViewModel to pass both patients and breadcrumbs
            var model = new EMTViewModel
            {
                Breadcrumbs = breadcrumbs,
                EMT = emt
            };


            return View(model);
              
        }



        [Route("/create-emt")]
        [HttpGet]
        public IActionResult CreateEMT()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
                new Tuple<string, string>("EMT", Url.Action("GetEMTs", "EMT")),
                new Tuple<string, string>("Create EMT", "")
            };

            ViewBag.Breadcrumbs = breadcrumbs;
           
            return View();
        }

        [Route("/create-emt")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEMT(CreateEMTViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
                new Tuple<string, string>("EMT", Url.Action("GetEMTs", "EMT")),
                new Tuple<string, string>("Create EMT", "")
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                // Return the same view with validation errors
                return View(model);
            }

            // Create a new User instance
            var user = new Users
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(null, model.Password), // Hash the password
                Role_Id = 4, // EMT role
                IsActive = true,
                RememberToken="Added by Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save the user to the database
            _context.Users.Add(user);
            _context.SaveChanges(); // Save the user first to get User ID

            // Create a new EMT instance
            var emt = new EMT
            {
                User_id = user.User_id,
                CertificationNumber = model.CertificationNumber,
                CertificationExpiryDate = model.CertificationExpiryDate,
                ContactNumber = model.ContactNumber,
                LicenseNumber = model.LicenseNumber,
                IsAvailable = model.IsAvailable,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save the EMT info to the database
            _context.EMTs.Add(emt);
            _context.SaveChanges();

            TempData["Message"] = "EMT is successfully added to the system.";
            return RedirectToAction("GetEMTs");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEMT(int userId)
        {
            // Find the EMT user by userId
            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user != null)
            {
                // Find the related EMT info if exists
                var emtInfo = _context.EMTs.FirstOrDefault(e => e.User_id == userId);

                // If EMT info exists, remove it
                if (emtInfo != null)
                {
                    _context.EMTs.Remove(emtInfo);
                }

                // Remove the user record
                _context.Users.Remove(user);

                // Save the changes to the database
                _context.SaveChanges();

                // Optionally: add a success message for feedback
                TempData["Message"] = "EMT and user information deleted successfully.";
            }

            // Redirect back to the EMT list
            return RedirectToAction("GetEMTs");
        }


    }
}

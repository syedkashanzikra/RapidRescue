using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Filters;
using RapidRescue.Models;
using RapidRescue.ViewModels;

namespace RapidRescue.Controllers
{
    [ServiceFilter(typeof(IsAdminLoggedIn))]
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
                return View(model);
            }
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

            
            _context.Users.Add(user);
            _context.SaveChanges();

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

            _context.EMTs.Add(emt);
            _context.SaveChanges();

            TempData["Message"] = "EMT is successfully added to the system.";
            return RedirectToAction("GetEMTs");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEMT(int userId)
        {

            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user != null)
            {

                var emtInfo = _context.EMTs.FirstOrDefault(e => e.User_id == userId);

                if (emtInfo != null)
                {
                    _context.EMTs.Remove(emtInfo);
                }

                _context.Users.Remove(user);


                _context.SaveChanges();


                TempData["Message"] = "EMT and user information deleted successfully.";
            }


            return RedirectToAction("GetEMTs");
        }


    }
}

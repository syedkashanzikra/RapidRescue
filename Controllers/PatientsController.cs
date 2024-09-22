using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Filters;
using RapidRescue.Models;
using RapidRescue.ViewModels;

namespace RapidRescue.Controllers
{
    [ServiceFilter(typeof(IsAdminLoggedIn))]
    public class PatientsController : Controller
    {
        private readonly RapidRescueContext _context;
        

        public PatientsController(RapidRescueContext context) 
        {
            _context = context;
        }

        [Route("/get-patients")]
        public IActionResult GetPatients()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Patients", "")
    };

            
            var patients = _context.Users.Where(u => u.Role_Id == 2).ToList();

            
            var model = new PatientsViewModel
            {
                Breadcrumbs = breadcrumbs,
                Patients = patients
            };

            return View(model);
        }


        [Route("/create-patients")]
        [HttpGet]
        public IActionResult CreatePatient()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Patient", Url.Action("GetPatients", "Patients")),
        new Tuple<string, string>("Create Patient", "")
    };

            // Store breadcrumbs in ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;

            return View();
        }



        [Route("/create-patients")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePatient(CreatePatientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Patient", Url.Action("GetPatients", "Patients")),
        new Tuple<string, string>("Create Patient", "")
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
                Role_Id = 2, 
                IsActive = true,
                RememberToken = "Patient Created by Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            
            _context.Users.Add(user);
            _context.SaveChanges(); 

            
            var patientInfo = new PatientsInfo
            {
                User_id = user.User_id,  
                MobileNumber = model.MobileNumber,
                Situation = model.Situation,
                PickupLocation = model.PickupLocation,
                AdditionalDetails = model.AdditionalDetails,
                Gender = model.Gender,
                IsEmergency = model.IsEmergency,
                RequestedTime = DateTime.UtcNow,
                
            };

            
            _context.PatientsInfo.Add(patientInfo);
            _context.SaveChanges();
            TempData["Message"] = "Patient is Added to The System Succesfully.";
            return RedirectToAction("GetPatients");
        }


        [Route("/edit-patient/{userId}")]
        [HttpGet]
        public IActionResult EditPatient(int userId)
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
        new Tuple<string, string>("Patient", Url.Action("GetPatients", "Patients")),
        new Tuple<string, string>("Edit Patient", "")
    };


            ViewBag.Breadcrumbs = breadcrumbs;
            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }


            var patientInfo = _context.PatientsInfo.FirstOrDefault(p => p.User_id == userId);

            if (patientInfo == null)
            {
                return NotFound("Patient information not found.");
            }

            
            var model = new CreatePatientViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNumber = patientInfo.MobileNumber,
                Situation = patientInfo.Situation,
                PickupLocation = patientInfo.PickupLocation,
                User_id = patientInfo.User_id,
                Patient_Id = patientInfo.Patient_Id,
                Gender = patientInfo.Gender,
                IsEmergency = patientInfo.IsEmergency,
                AdditionalDetails = patientInfo.AdditionalDetails,
                Password = user.Password,
                ConfirmPassword =user.Password
            };

            return View(model);
        }

        [Route("/edit-patient/{userId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPatient(int userId, CreatePatientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("Home", Url.Action("Home", "Home")),
            new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
            new Tuple<string, string>("Patient", Url.Action("GetPatients", "Patients")),
            new Tuple<string, string>("Edit Patient", "")
        };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);
            if (user == null) return NotFound("User not found.");

            var patientInfo = _context.PatientsInfo.FirstOrDefault(p => p.User_id == userId);
            if (patientInfo == null) return NotFound("Patient information not found.");

            
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UpdatedAt = DateTime.UtcNow;

            
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<Users>();
                user.Password = passwordHasher.HashPassword(user, model.Password);
            }
            
            patientInfo.MobileNumber = model.MobileNumber;
            patientInfo.Situation = model.Situation;
            patientInfo.PickupLocation = model.PickupLocation;
            patientInfo.Gender = model.Gender;
            patientInfo.IsEmergency = model.IsEmergency;
            patientInfo.AdditionalDetails = model.AdditionalDetails;

            _context.Users.Update(user);
            _context.PatientsInfo.Update(patientInfo);
            _context.SaveChanges();

            return RedirectToAction("GetPatients");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePatient(int userId)
        {

            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user != null)
            {

                var patientInfo = _context.PatientsInfo.FirstOrDefault(p => p.User_id == userId);


                if (patientInfo != null)
                {
                    _context.PatientsInfo.Remove(patientInfo);
                }

                _context.Users.Remove(user);


                _context.SaveChanges();
                TempData["Message"] = "Patient and user information deleted successfully.";
            }

            return RedirectToAction("GetPatients");
        }


    }
}

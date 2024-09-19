using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Filters;
using RapidRescue.Models;
using RapidRescue.ViewModels;

namespace RapidRescue.Controllers
{
    //[ServiceFilter(typeof(IsAdminLoggedIn))]
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

            // Fetching users with Role_Id == 2
            var patients = _context.Users.Where(u => u.Role_Id == 2).ToList();

            // Create a ViewModel to pass both patients and breadcrumbs
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
                // Return the same view with validation errors if the model state is invalid
                return View(model);
            }

            // Initialize the password hasher
            var passwordHasher = new PasswordHasher<Users>();

            // Create a new User instance
            var user = new Users
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = passwordHasher.HashPassword(null, model.Password), // Use the password hasher here
                Role_Id = 2, // Patient role
                IsActive = true,
                RememberToken = "Patient Created by Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save the User to the database
            _context.Users.Add(user);
            _context.SaveChanges();  // Save user first to get the User ID for the PatientsInfo record

            // Create a new PatientsInfo instance
            var patientInfo = new PatientsInfo
            {
                User_id = user.User_id,  // Use the ID of the recently created user
                MobileNumber = model.MobileNumber,
                Situation = model.Situation,
                PickupLocation = model.PickupLocation,
                AdditionalDetails = model.AdditionalDetails,
                Gender = model.Gender,
                IsEmergency = model.IsEmergency,
                RequestedTime = DateTime.UtcNow,
                
            };

            // Save the PatientsInfo to the database
            _context.PatientsInfo.Add(patientInfo);
            _context.SaveChanges();
            TempData["Message"] = "Patient is Added to The System Succesfully.";
            // Redirect to the patient list or success page
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

            // Store breadcrumbs in ViewBag
            ViewBag.Breadcrumbs = breadcrumbs;

            // Fetch the user information
            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            // Check if user exists
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Fetch the patient info using the User_id
            var patientInfo = _context.PatientsInfo.FirstOrDefault(p => p.User_id == userId);

            // Check if patient info exists
            if (patientInfo == null)
            {
                return NotFound("Patient information not found.");
            }

            // Populate the view model with existing data
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

            // Update user details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UpdatedAt = DateTime.UtcNow;

            // Only update the password if the model has a new password
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<Users>();
                user.Password = passwordHasher.HashPassword(user, model.Password);
            }
            // Otherwise, leave the password unchanged (use the old password)

            // Update patient info
            patientInfo.MobileNumber = model.MobileNumber;
            patientInfo.Situation = model.Situation;
            patientInfo.PickupLocation = model.PickupLocation;
            patientInfo.Gender = model.Gender;
            patientInfo.IsEmergency = model.IsEmergency;
            patientInfo.AdditionalDetails = model.AdditionalDetails;

            // Save changes
            _context.Users.Update(user);
            _context.PatientsInfo.Update(patientInfo);
            _context.SaveChanges();

            return RedirectToAction("GetPatients");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePatient(int userId)
        {
            // Find the user by userId
            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);

            if (user != null)
            {
                // Find the related patient info
                var patientInfo = _context.PatientsInfo.FirstOrDefault(p => p.User_id == userId);

                // If patient info exists, remove it
                if (patientInfo != null)
                {
                    _context.PatientsInfo.Remove(patientInfo);
                }

                // Remove the user
                _context.Users.Remove(user);

                // Save changes to the database
                _context.SaveChanges();

                // Optionally: add a success message for feedback
                TempData["Message"] = "Patient and user information deleted successfully.";
            }

            // Redirect back to the patients list
            return RedirectToAction("GetPatients");
        }


    }
}

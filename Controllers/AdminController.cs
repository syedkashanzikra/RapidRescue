using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;
using RapidRescue.Filters;

namespace RapidRescue.Controllers
{
    [ServiceFilter(typeof(UserSessionCheckAttribute))]
    public class AdminController : Controller
    {
        private readonly RapidRescueContext _context;
        public AdminController(RapidRescueContext context)
        {
            _context = context;
        }

        [Route("/admin")]
        public async Task<IActionResult> Admin()
        {
            // Get the user_id and role_id from the session
            var userId = HttpContext.Session.GetInt32("user_id");
            var roleId = HttpContext.Session.GetInt32("role_id");

            

            // Retrieve the user from the database using the user_id
            var user = await _context.Users
                                     .Where(u => u.User_id == userId.Value)
                                     .Select(u => new { u.FirstName, u.LastName })  // Select only the required fields
                                     .FirstOrDefaultAsync();

          

            // Concatenate first name and last name to form the full name
            var fullName = $"{user.FirstName} {user.LastName}";

            // Pass the full name to the view using ViewBag
            ViewBag.FullName = fullName;

            // Determine the dashboard name based on role_id
            string dashboardName;
            switch (roleId.Value)
            {
                case 1:
                    dashboardName = "Admin Dashboard";
                    break;
                case 2:
                    dashboardName = "Patient Dashboard";
                    break;
                case 3:
                    dashboardName = "Driver Dashboard";
                    break;
                case 4:
                    dashboardName = "EMT Dashboard";
                    break;
                default:
                    return RedirectToAction("Home", "Home");
            }

            // Create breadcrumbs with dynamic dashboard name
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>(dashboardName, "")
    };

            // Pass breadcrumbs to the view
            return View(breadcrumbs);
        }



    }
}

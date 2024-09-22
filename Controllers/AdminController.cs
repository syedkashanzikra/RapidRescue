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
            
            var userId = HttpContext.Session.GetInt32("user_id");
            var roleId = HttpContext.Session.GetInt32("role_id");

            var user = await _context.Users
                                     .Where(u => u.User_id == userId.Value)
                                     .Select(u => new { u.FirstName, u.LastName })  
                                     .FirstOrDefaultAsync();

          


            var fullName = $"{user.FirstName} {user.LastName}";


            ViewBag.FullName = fullName;


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


            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>(dashboardName, "")
    };
            // Fetch counts for dashboard cards
            ViewBag.UserCount = await _context.Users.CountAsync(); // Total users count
            ViewBag.PatientCount = await _context.PatientsInfo.CountAsync(); // Total patients
            ViewBag.DriverCount = await _context.DriverInfo.CountAsync(); // Total drivers
            ViewBag.EMTCount = await _context.EMTs.CountAsync(); // Total EMTs
            ViewBag.RequestCount = await _context.Requests.CountAsync(); // Total requests (appointments)


            var malePatients = await _context.PatientsInfo.Where(p => p.Gender == "Male").CountAsync();
            var femalePatients = await _context.PatientsInfo.Where(p => p.Gender == "Female").CountAsync();
            var totalPatients = malePatients + femalePatients;

            ViewBag.MalePercentage = totalPatients > 0 ? (malePatients * 100) / totalPatients : 0;
            ViewBag.FemalePercentage = totalPatients > 0 ? (femalePatients * 100) / totalPatients : 0;


            ViewBag.RecentPatients = await _context.PatientsInfo
                                      .OrderByDescending(p => p.RequestedTime)
                                      .Take(5)  // Get the latest 5 patients
                                      .ToListAsync();

            // Fetch recent requests (replace with actual model properties)
            ViewBag.RecentRequests = await _context.Requests
                                                .OrderByDescending(r => r.RequestedAt)
                                                .Take(5)  // Get the latest 5 requests
                                                .ToListAsync();

            ViewBag.RecentRequests = await _context.Requests
    .Include(r => r.DriverInfo) // Ensure DriverInfo is included
    .ThenInclude(d => d.Users)  // Ensure Users related to DriverInfo are included
    .OrderByDescending(r => r.RequestedAt)
    .Take(5)
    .ToListAsync();

            return View(breadcrumbs);
        }

     

    }
}

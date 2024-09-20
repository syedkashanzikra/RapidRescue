using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;
using RapidRescue.Filters;

namespace RapidRescue.Controllers
{
    //[ServiceFilter(typeof(UserSessionCheckAttribute))]
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


            return View(breadcrumbs);
        }

        [Route("/admin/get-drivers-map")]
        public IActionResult DriversLocation()
        {
            return View();
        }

    }
}

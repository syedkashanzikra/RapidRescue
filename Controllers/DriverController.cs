using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.ViewModels;

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

            // Create a ViewModel to pass both patients and breadcrumbs
            var model = new DriverViewModel
            {
                Breadcrumbs = breadcrumbs,
                Drivers = drivers
            };

            return View(model);
        }


       
    }
}

using Microsoft.AspNetCore.Mvc;
using RapidRescue.Filters;

namespace RapidRescue.Controllers
{


    [ServiceFilter(typeof(IsAdminLoggedIn))]
    public class AdminController : Controller
    {
        [Route("/admin")]
        public IActionResult Admin()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Admin Dashboard", "")
    };
            return View(breadcrumbs);
        }


    }
}

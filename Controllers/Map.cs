using Microsoft.AspNetCore.Mvc;

namespace RapidRescue.Controllers
{
    public class Map : Controller
    {
        [Route("/get-map")]
        public IActionResult GetMap()
        {
            return View();
        }
    }
}

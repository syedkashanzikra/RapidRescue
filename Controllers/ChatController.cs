using Microsoft.AspNetCore.Mvc;

namespace RapidRescue.Controllers
{
    public class ChatController : Controller
    {
        [Route("/chat")]
        public IActionResult Index()
        {
            var breadcrumbs = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("Home", Url.Action("Home", "Home")),
        new Tuple<string, string>("Chat", "")
    };
            return View(breadcrumbs);
        }
    }
}

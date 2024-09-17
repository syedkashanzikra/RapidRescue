using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;

namespace RapidRescue.Controllers
{
    public class UserController : Controller
    {
        private readonly RapidRescueContext _context;
        private readonly IConfiguration _configuration;


        public UserController(RapidRescueContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("/register")]
        public IActionResult Register_User()
        {

            return View();
        }

        [HttpGet]
        public IActionResult RegistrationConfirmation()
        {
            return View();  
        }


        [HttpGet]
        [Route("/login")]
        public IActionResult Login_User()
        {

            return View();
        }

    }
}

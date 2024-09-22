using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;

namespace RapidRescue.Filters
{
    public class DriverCheckingRole : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RapidRescueContext _context;

        public DriverCheckingRole(IHttpContextAccessor httpContextAccessor, RapidRescueContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Check if session contains user_id
            var userId = httpContext.Session.GetInt32("user_id");
            if (!userId.HasValue)
            {
                // If session does not contain user_id, redirect to Login
                context.Result = new RedirectToActionResult("Login_User", "User", null);
                return;
            }

            // Fetch user from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_id == userId.Value);

            if (user == null)
            {
                // If user does not exist in the database, clear the session and redirect to Home
                httpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

            // Check if user's Role_Id is 1 (Admin) or 3 (Driver)
            if (user.Role_Id != 1 && user.Role_Id != 3)
            {
                // If the role is neither 1 nor 3, redirect to Home
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

            // Allow the action to proceed if Role_Id is 1 (Admin) or 3 (Driver)
            await next();
        }
    }
}

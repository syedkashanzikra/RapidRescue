using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using Microsoft.EntityFrameworkCore;

namespace RapidRescue.Filters
{
    public class UserSessionCheckAttribute : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RapidRescueContext _context;

        public UserSessionCheckAttribute(IHttpContextAccessor httpContextAccessor, RapidRescueContext context)
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
                // If no user_id in session, redirect to Home/Index
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

            // Find the user by userId
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_id == userId.Value);

            if (user == null)
            {
                // If user is not found, clear session and redirect to Home/Index
                httpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

            // If everything is OK, proceed to the requested action
            await next();
        }
    }
}

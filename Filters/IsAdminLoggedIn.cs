using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;

namespace RapidRescue.Filters
{

    public class IsAdminLoggedIn : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RapidRescueContext _context;

        public IsAdminLoggedIn(IHttpContextAccessor httpContextAccessor, RapidRescueContext context)
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
         
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

     
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_id == userId.Value);

            if (user == null)
            {
           
                httpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

      
            if (user.Role_Id != 1)
            {
            
                context.Result = new RedirectToActionResult("Home", "Home", null);
                return;
            }

            await next();
        }
    }
}
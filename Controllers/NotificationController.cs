using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using System.Linq;
using System.Threading.Tasks;

namespace RapidRescue.Controllers
{
    public class NotificationController : Controller
    {
        private readonly RapidRescueContext _context;

        public NotificationController(RapidRescueContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getnotifications")]
        public async Task<IActionResult> GetLatestNotifications()
        {
            var notifications = _context.Notifications
                .OrderByDescending(n => n.CreatedAt) // Sort by latest first
                .Take(10) // Take the latest 10 notifications (you can adjust this as needed)
                .Select(n => new
                {
                    n.NotificationId,
                    n.NotificationType,
                    n.NotificationMessage,
                    CreatedAt = n.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss") // Format the date if needed
                })
                .ToList();

            return Json(notifications); // Return the notifications as JSON
        }
    }
}

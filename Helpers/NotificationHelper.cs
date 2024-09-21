using RapidRescue.Context;
using RapidRescue.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RapidRescue.Hubs;

namespace RapidRescue.Helpers
{
    public class NotificationHelper
    {
        private readonly RapidRescueContext _context;
        private readonly IHubContext<DriverLocationHub> _hubContext;

        public NotificationHelper(RapidRescueContext context, IHubContext<DriverLocationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // Method to create a new notification and broadcast it via SignalR
        public async Task CreateNotification(string notificationType, string notificationMessage)
        {
            var notification = new Notification
            {
                NotificationType = notificationType,
                NotificationMessage = notificationMessage
            };

            // Save the notification in the database
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send notification via SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);
        }
    }
}

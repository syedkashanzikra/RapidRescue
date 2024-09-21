using Microsoft.AspNetCore.SignalR;
using RapidRescue.Hubs;
using RapidRescue.Context;
using RapidRescue.Models;
using System.Threading.Tasks;

namespace RapidRescue.Helpers
{
    public class NotificationHelper
    {
        private readonly RapidRescueContext _context;
        private readonly IHubContext<DriverLocationHub> _driverLocationHub;

        public NotificationHelper(RapidRescueContext context, IHubContext<DriverLocationHub> driverLocationHub)
        {
            _context = context;
            _driverLocationHub = driverLocationHub;
        }

        public async Task CreateNotification(string notificationType, string notificationMessage)
        {
            var notification = new Notification
            {
                NotificationType = notificationType,
                NotificationMessage = notificationMessage
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Trigger notification sound after saving the notification
            await _driverLocationHub.Clients.All.SendAsync("PlayNotificationSound");
        }
    }
}

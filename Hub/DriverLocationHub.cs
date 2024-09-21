using Microsoft.AspNetCore.SignalR;
using RapidRescue.Context;
using System.Threading.Tasks;

namespace RapidRescue.Hubs
{
    public class DriverLocationHub : Hub
    {
        private readonly RapidRescueContext _context;

        public DriverLocationHub(RapidRescueContext context)
        {
            _context = context;
        }

        // Existing Methods for Driver Location Tracking
        public async Task UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            var driver = _context.DriverInfo.FirstOrDefault(d => d.DriverId == driverId);

            if (driver != null)
            {
                driver.Latitude = latitude;
                driver.Longitude = longitude;
                driver.UpdatedAt = DateTime.UtcNow;

                _context.DriverInfo.Update(driver);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
            }
        }

        public async Task SendLocationUpdate(int driverId, double latitude, double longitude)
        {
            using (var scope = Context.GetHttpContext().RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RapidRescueContext>();

                var driver = dbContext.DriverInfo.FirstOrDefault(d => d.DriverId == driverId);
                if (driver != null)
                {
                    driver.Latitude = latitude;
                    driver.Longitude = longitude;
                    driver.UpdatedAt = DateTime.UtcNow;

                    dbContext.DriverInfo.Update(driver);
                    await dbContext.SaveChangesAsync();
                }
            }

            await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
        }

        public async Task StartLocationTracking(int driverId)
        {
            await Clients.All.SendAsync("TrackingStarted", driverId);
        }

        public async Task StopLocationTracking(int driverId)
        {
            await Clients.All.SendAsync("TrackingStopped", driverId);
        }

        // *** NEW: Notification Sound Broadcast ***
        public async Task PlayNotificationSound()
        {
            // Broadcast to all connected clients to play the notification sound
            await Clients.All.SendAsync("PlayNotificationSound");
        }
    }
}

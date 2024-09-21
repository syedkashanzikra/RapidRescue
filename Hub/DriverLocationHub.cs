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

        //public async Task UpdateDriverLocation(int driverId, double latitude, double longitude)
        //{
        //    // Fetch the driver from the database
        //    var driver = _context.DriverInfo.FirstOrDefault(d => d.DriverId == driverId);

        //    if (driver != null)
        //    {
        //        // Update the driver's location in the database
        //        driver.Latitude = latitude;
        //        driver.Longitude = longitude;
        //        driver.UpdatedAt = DateTime.UtcNow;

        //        // Save changes to the database
        //        _context.DriverInfo.Update(driver);
        //        await _context.SaveChangesAsync();

        //        // Notify all clients about this driver's location update
        //        await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
        //    }
        //}

        public async Task UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            // Fetch the driver from the database
            var driver = _context.DriverInfo.FirstOrDefault(d => d.DriverId == driverId);

            if (driver != null)
            {
                // Update the driver's location in the database
                driver.Latitude = latitude;
                driver.Longitude = longitude;
                driver.UpdatedAt = DateTime.UtcNow;

                // Save changes to the database
                _context.DriverInfo.Update(driver);
                await _context.SaveChangesAsync();

                // Notify all clients about this driver's location update
                await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
            }
        }



        public async Task SendLocationUpdate(int driverId, double latitude, double longitude)
        {
            // Store the location in the database
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

            // Broadcast the location update to all connected clients
            await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
        }

        // Start tracking real-time location
        public async Task StartLocationTracking(int driverId)
        {
            // Logic to start tracking the driver's location
            await Clients.All.SendAsync("TrackingStarted", driverId);
        }

        // Stop tracking real-time location
        public async Task StopLocationTracking(int driverId)
        {
            // Logic to stop tracking the driver's location
            await Clients.All.SendAsync("TrackingStopped", driverId);
        }
    }
}

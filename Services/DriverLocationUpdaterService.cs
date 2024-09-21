using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidRescue.Context;
using Microsoft.AspNetCore.SignalR;
using RapidRescue.Hubs;
using System.Linq;

namespace RapidRescue.Services
{
    public class DriverLocationUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DriverLocationUpdaterService> _logger;
        private readonly IHubContext<DriverLocationHub> _hubContext;

        public DriverLocationUpdaterService(IServiceProvider serviceProvider, ILogger<DriverLocationUpdaterService> logger, IHubContext<DriverLocationHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateActiveDriverLocations();
                await Task.Delay(5000, stoppingToken); // Wait for 5 seconds before the next update
            }
        }

        private async Task UpdateActiveDriverLocations()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RapidRescueContext>();
                var activeDrivers = dbContext.DriverInfo.Where(d => d.IsActive).ToList();

                foreach (var driver in activeDrivers)
                {
                    // Fetch the latest location from the database instead of hardcoded values
                    var latestLocation = dbContext.DriverInfo
                                         .Where(d => d.DriverId == driver.DriverId)
                                         .Select(d => new { d.Latitude, d.Longitude })
                                         .FirstOrDefault();

                    if (latestLocation != null)
                    {
                        driver.Latitude = latestLocation.Latitude;
                        driver.Longitude = latestLocation.Longitude;
                        driver.UpdatedAt = DateTime.UtcNow;

                        // Broadcast the new location to all connected clients using SignalR
                        await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", driver.DriverId, driver.Latitude, driver.Longitude);

                        _logger.LogInformation($"Driver {driver.DriverId} updated with Latitude: {driver.Latitude}, Longitude: {driver.Longitude}");
                    }
                }

                await dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Active driver locations updated at: {time}", DateTimeOffset.Now);
        }

        // This method should fetch the latest GPS coordinates from the database or an external source
        private Location GetDriverLocationFromDatabase(int driverId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RapidRescueContext>();
                var driverLocation = dbContext.DriverInfo
                    .Where(d => d.DriverId == driverId)
                    .Select(d => new Location
                    {
                        Latitude = d.Latitude,
                        Longitude = d.Longitude
                    })
                    .FirstOrDefault();

                return driverLocation;
            }
        }

        // This method checks if the location has changed by a significant amount
        private bool HasLocationChanged(double? oldLat, double? oldLng, double? newLat, double? newLng)
        {
            if (!oldLat.HasValue || !oldLng.HasValue || !newLat.HasValue || !newLng.HasValue)
                return true; // No location data to compare

            // Calculate the difference between the old and new coordinates (in decimal degrees)
            double latDiff = Math.Abs(oldLat.Value - newLat.Value);
            double lngDiff = Math.Abs(oldLng.Value - newLng.Value);

            // Threshold for significant movement (e.g., 0.0001 degrees ~11 meters)
            return latDiff > 0.0001 || lngDiff > 0.0001;
        }
    }

    public class Location
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

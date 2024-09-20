using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidRescue.Context;
using Microsoft.AspNetCore.SignalR;
using RapidRescue.Hubs; // Import your SignalR Hub
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
                var dbContext = scope.ServiceProvider.GetRequiredService<RapidRescueContext>(); // Replace with your DbContext
                var activeDrivers = dbContext.DriverInfo.Where(d => d.IsActive).ToList();

                foreach (var driver in activeDrivers)
                {
                    // Use real or known latitude and longitude values
                    driver.Latitude = GetRealTimeLatitude(driver.DriverId); // Fetch from GPS or use static known values
                    driver.Longitude = GetRealTimeLongitude(driver.DriverId); // Fetch from GPS or use static known values
                    driver.UpdatedAt = DateTime.UtcNow;

                    // Broadcast the new location to all connected clients using SignalR
                    await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", driver.DriverId, driver.Latitude, driver.Longitude);

                    _logger.LogInformation($"Driver {driver.DriverId} updated with Latitude: {driver.Latitude}, Longitude: {driver.Longitude}");
                }

                await dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Active driver locations updated at: {time}", DateTimeOffset.Now);
        }

        // For now, replace these with fixed coordinates for testing. Replace them later with actual GPS fetching logic.
        private double GetRealTimeLatitude(int driverId)
        {
            // Use known coordinates for testing
            return 24.873589079144057; // Example: Karachi Latitude
        }

        private double GetRealTimeLongitude(int driverId)
        {
            // Use known coordinates for testing
            return 67.02940765987226; // Example: Karachi Longitude
        }
    }
}

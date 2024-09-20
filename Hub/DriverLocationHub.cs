using Microsoft.AspNetCore.SignalR;

namespace RapidRescue.Hubs
{
    public class DriverLocationHub : Hub
    {
        public async Task UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            // Notify all clients about this driver's location update
            await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
        }
    }
}

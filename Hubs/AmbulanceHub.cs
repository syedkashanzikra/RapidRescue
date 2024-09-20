using Microsoft.AspNetCore.SignalR;

namespace RapidRescue.Hubs { 

public class AmbulanceHub : Hub
{
    public async Task UpdateDriverLocation(string driverId, double latitude, double longitude)
    {
        // Send the driver's location to all clients
        await Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);
    }
}
}

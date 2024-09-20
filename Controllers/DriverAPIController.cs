using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RapidRescue.Context;
using RapidRescue.Hubs;
using System.Linq;
using System.Threading.Tasks;

namespace RapidRescue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverAPIController : ControllerBase
    {
        private readonly RapidRescueContext _context;
        private readonly IHubContext<DriverLocationHub> _hubContext;

        public DriverAPIController(RapidRescueContext context, IHubContext<DriverLocationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateDriverLocation(int driverId, double latitude, double longitude)
        {
            var driver = _context.DriverInfo.FirstOrDefault(d => d.DriverId == driverId && d.IsActive);
            if (driver == null) return NotFound();

            driver.Latitude = latitude;
            driver.Longitude = longitude;
            driver.UpdatedAt = DateTime.UtcNow;

            _context.DriverInfo.Update(driver);
            await _context.SaveChangesAsync();

            // Broadcast the new location using SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", driverId, latitude, longitude);

            return Ok();
        }
    }
}

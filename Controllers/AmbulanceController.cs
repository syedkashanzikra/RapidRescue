using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RapidRescue.Context;
using RapidRescue.Models;
using RapidRescue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidRescue.Controllers
{
    public class AmbulanceController : Controller
    {
        private readonly RapidRescueContext _context;

        private readonly ILogger<AmbulanceController> _logger;

        public AmbulanceController(RapidRescueContext context, ILogger<AmbulanceController> logger)
        {
            _context = context;
            _logger = logger;
        }


        private List<Tuple<string, string>> GetBreadcrumbs(string currentPage, string action, string controller = "Ambulance", string parentBreadcrumbAction = null)
        {
            var breadcrumbs = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Home", Url.Action("Home", "Home")),
                new Tuple<string, string>("Admin", Url.Action("Admin", "Admin")),
            };
            if (!string.IsNullOrEmpty(parentBreadcrumbAction))
            {
                breadcrumbs.Add(new Tuple<string, string>("Ambulance", Url.Action(parentBreadcrumbAction, controller)));
            }

            breadcrumbs.Add(new Tuple<string, string>(currentPage, null));

            return breadcrumbs;
        }


        [Route("/get-ambulances")]
        public async Task<IActionResult> GetAmbulances()
        {
            var breadcrumbs = GetBreadcrumbs("Ambulance List", "Index");

            var ambulances = await _context.Ambulances.Include(a => a.DriverInfo).ThenInclude(d => d.Users).ToListAsync();

            var model = new AmbulanceViewModel
            {
                Breadcrumbs = breadcrumbs,
                Ambulances = ambulances
            };

            return View(model);
        }

        [Route("/create-ambulance")]
        [HttpGet]
        public IActionResult CreateAmbulance()
        {
            var breadcrumbs = GetBreadcrumbs("Create Ambulance", "Create", "Ambulance", "GetAmbulances");

            ViewBag.Breadcrumbs = breadcrumbs;
            ViewBag.Drivers = new SelectList(_context.DriverInfo.Include(d => d.Users), "DriverId", "Users.FirstName");
            return View();
        }

        [Route("/create-ambulance")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAmbulance(CreateAmbulanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = GetBreadcrumbs("Create Ambulance", "Create", "Ambulance", "GetAmbulances");
                ViewBag.Breadcrumbs = breadcrumbs;
                ViewBag.Drivers = new SelectList(_context.DriverInfo.Include(d => d.Users), "DriverId", "Users.FirstName");
                return View(model);
            }

            var ambulance = new Ambulance
            {
                VehicleNumber = model.VehicleNumber,
                EquipmentLevel = model.EquipmentLevel,
                DriverId = model.DriverId
            };

            _context.Ambulances.Add(ambulance);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Ambulance added successfully.";
            return RedirectToAction("GetAmbulances");
        }

        [Route("/edit-ambulance/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditAmbulance(int id)
        {
            var ambulance = await _context.Ambulances.FindAsync(id);
            if (ambulance == null)
            {
                return NotFound("Ambulance not found.");
            }

            var model = new EditAmbulanceViewModel
            {
                AmbulanceId = ambulance.AmbulanceId,
                VehicleNumber = ambulance.VehicleNumber,
                EquipmentLevel = ambulance.EquipmentLevel,
                DriverId = ambulance.DriverId
            };

            var breadcrumbs = GetBreadcrumbs("Edit Ambulance", "Edit", "Ambulance", "GetAmbulances");
            ViewBag.Breadcrumbs = breadcrumbs;
            ViewBag.Drivers = new SelectList(_context.DriverInfo.Include(d => d.Users), "DriverId", "Users.FirstName", ambulance.DriverId);
            return View(model);
        }

        [Route("/edit-ambulance/{id}")]
        [HttpPost, ActionName("EditAmbulance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAmbulance(int id, EditAmbulanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var breadcrumbs = GetBreadcrumbs("Edit Ambulance", "Edit", "Ambulance", "GetAmbulances");
                ViewBag.Breadcrumbs = breadcrumbs;
                ViewBag.Drivers = new SelectList(_context.DriverInfo.Include(d => d.Users), "DriverId", "Users.FirstName", model.DriverId);
                return View(model);
            }

            var ambulance = await _context.Ambulances.FindAsync(id);
            if (ambulance == null)
            {
                return NotFound("Ambulance not found.");
            }

            ambulance.VehicleNumber = model.VehicleNumber;
            ambulance.EquipmentLevel = model.EquipmentLevel;
            ambulance.DriverId = model.DriverId;

            _context.Update(ambulance);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Ambulance updated successfully.";
            return RedirectToAction("GetAmbulances");
        }

        [Route("/delete-ambulance/{id}")]
        public async Task<IActionResult> DeleteAmbulance(int? id)
        {
            if (id == null)
            {
                return NotFound("Ambulance not found.");
            }

            var ambulance = await _context.Ambulances.Include(a => a.DriverInfo).FirstOrDefaultAsync(m => m.AmbulanceId == id);
            if (ambulance == null)
            {
                return NotFound("Ambulance not found.");
            }

            var breadcrumbs = GetBreadcrumbs("Delete Ambulance", "Delete");
            ViewBag.Breadcrumbs = breadcrumbs;
            return View(ambulance);
        }

        [HttpPost, ActionName("DeleteAmbulance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ambulance = await _context.Ambulances.FindAsync(id);
            if (ambulance != null)
            {
                _context.Ambulances.Remove(ambulance);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Ambulance deleted successfully.";
            }

            return RedirectToAction("GetAmbulances");
        }

        private bool AmbulanceExists(int id)
        {
            return _context.Ambulances.Any(e => e.AmbulanceId == id);
        }

        [HttpPost]
        public async Task<IActionResult> RequestAmbulance([FromBody] AmbulanceRequest model)
        {
            try
            {
                var requestId = Guid.NewGuid().ToString();

                // First fetch all active drivers from the database (this will execute the query in SQL)
                var activeDrivers = _context.DriverInfo
                    .Where(d => d.IsActive && d.Latitude != null && d.Longitude != null)
                    .AsEnumerable();  // This pulls the data into memory, allowing us to use GetDistance

                // Now perform the distance calculation in memory
                var nearestDriver = activeDrivers
                    .OrderBy(d => GetDistance(d.Latitude.Value, d.Longitude.Value, model.PatientLat, model.PatientLng))
                    .FirstOrDefault();

                if (nearestDriver != null)
                {
                    // Save the ambulance request in the database
                    var ambulanceRequest = new AmbulanceRequest
                    {
                        RequestId = requestId,
                        DriverId = nearestDriver.DriverId.ToString(),
                        PatientLat = model.PatientLat,
                        PatientLng = model.PatientLng,
                        RequestTime = DateTime.UtcNow
                    };

                    _context.AmbulanceRequests.Add(ambulanceRequest);
                    await _context.SaveChangesAsync();

                    // Return driver information and request ID to the client
                    return Ok(new
                    {
                        driverId = nearestDriver.DriverId,
                        eta = CalculateEstimatedArrival(nearestDriver, model.PatientLat, model.PatientLng),
                        requestId = requestId
                    });
                }

                return NotFound("No active drivers available.");
            }
            catch (IOException ex)
            {
                // Log the IO exception with more context
                _logger.LogWarning(ex, "Request was canceled by the client.");
                return BadRequest("Request was canceled.");
            }
            catch (Exception ex)
            {
                // Log the detailed exception information
                _logger.LogError(ex, "An error occurred while processing the ambulance request. Request data: {Model}", model);
                return StatusCode(500, "An error occurred on the server.");
            }
        }


        // Helper method to calculate the estimated arrival time of the ambulance
        private double CalculateEstimatedArrival(DriverInfo driver, double patientLat, double patientLng)
        {
            double distance = GetDistance(driver.Latitude.Value, driver.Longitude.Value, patientLat, patientLng);
            double averageSpeed = 50;  // Example speed in km/h
            return (distance / averageSpeed) * 60;  // Return ETA in minutes
        }

        // Haversine formula to calculate distance between two lat/lng points
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371;  // Radius of the Earth in km
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;
            return distance;
        }

    }
}

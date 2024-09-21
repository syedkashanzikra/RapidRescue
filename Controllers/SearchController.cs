using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Controllers
{
    public class SearchController : Controller
    {
        private readonly RapidRescueContext _context;

        public SearchController(RapidRescueContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchAll(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new List<DriverInfo>()); // Return an empty list if query is empty
            }

            var drivers = _context.DriverInfo
                .Where(d => d.PhoneNumber.Contains(query) || d.LicenseNumber.Contains(query) || d.Address.Contains(query) || d.Users.FirstName.Contains(query) || d.Users.LastName.Contains(query))  // Customize search based on your fields
                .Select(d => new
                {
                    d.DriverId,
                    d.PhoneNumber,
                    d.LicenseNumber,
                    d.LicenseExpiryDate,
                    d.Address,
                    d.VehicleAssigned,
                    d.DateOfHire,
                    d.IsActive,
                    d.Latitude,
                    d.Longitude,
                    d.Users.FirstName,
                    d.Users.LastName,// Assuming Users table has a Name field
                })
                .ToList();
            // Search in EMT
            var emts = _context.EMTs
                .Where(e => e.ContactNumber.Contains(query) || e.CertificationNumber.Contains(query) || e.LicenseNumber.Contains(query) || e.Address.Contains(query) || e.Users.FirstName.Contains(query) || e.Users.LastName.Contains(query))
                .Select(e => new
                {
                    e.EMT_Id,
                    e.CertificationNumber,
                    e.CertificationExpiryDate,
                    e.ContactNumber,
                    e.LicenseNumber,
                    e.IsAvailable,
                    e.Address,
                    e.Users.FirstName,
                    e.Users.LastName
                })
                .ToList();

            // Search in PatientsInfo
            var patients = _context.PatientsInfo
                .Where(p => p.MobileNumber.Contains(query) || p.Situation.Contains(query) || p.PickupLocation.Contains(query) || p.Users.FirstName.Contains(query) || p.Users.LastName.Contains(query))
                .Select(p => new
                {
                    p.Patient_Id,
                    p.MobileNumber,
                    p.Situation,
                    p.PickupLocation,
                    p.RequestedTime,
                    p.AdditionalDetails,
                    p.Gender,
                    p.IsEmergency,
                    p.Users.FirstName,
                    p.Users.LastName
                })
                .ToList();

            // Return results for all three models
            return Json(new { drivers = drivers, emts = emts, patients = patients });
        }
    }
}

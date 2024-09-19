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

        public AmbulanceController(RapidRescueContext context)
        {
            _context = context;
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
    }
}

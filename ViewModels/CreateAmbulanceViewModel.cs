using System.ComponentModel.DataAnnotations;

namespace RapidRescue.ViewModels
{
    public class CreateAmbulanceViewModel
    {
        [Required]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        [Required]
        [Display(Name = "Equipment Level")]
        public string EquipmentLevel { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public int DriverId { get; set; } // This will hold the selected driver from the dropdown
    }
}

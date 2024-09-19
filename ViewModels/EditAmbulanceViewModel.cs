using System.ComponentModel.DataAnnotations;

namespace RapidRescue.ViewModels
{
    public class EditAmbulanceViewModel
    {
        [Required]
        public int AmbulanceId { get; set; } // Hidden field to keep track of the ambulance being edited

        [Required]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        [Required]
        [Display(Name = "Equipment Level")]
        public string EquipmentLevel { get; set; }

        [Required]
        [Display(Name = "Driver")]
        public int DriverId { get; set; } // To hold the selected driver during edit
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRescue.Models
{
    public class Ambulance
    {
        [Key]
        public int AmbulanceId { get; set; }

        [Required]
        [StringLength(50)]
        public string VehicleNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string EquipmentLevel { get; set; } // Basic or Advanced equipment

        [Required]
        public int DriverId { get; set; }  // Foreign key for Driver

        [ForeignKey("DriverId")]
        public DriverInfo DriverInfo { get; set; }  // Navigation property
    }
}

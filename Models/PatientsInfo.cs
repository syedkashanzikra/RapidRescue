using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRescue.Models
{
    public class PatientsInfo
    {
        [Key]
        public int Patient_Id { get; set; }

        [Required]
        public int User_id { get; set; }

        [ForeignKey("User_id")]
        public Users Users { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string MobileNumber { get; set; }  // Contact number of the patient

        [Required]
        [StringLength(255)]
        public string Situation { get; set; }  // Description of the medical situation

        [Required]
        [StringLength(255)]
        public string? PickupLocation { get; set; }  // Where the patient is currently located


        [DataType(DataType.DateTime)]
        [Required]
        public DateTime RequestedTime { get; set; } = DateTime.UtcNow;  // Time when the ambulance was requested

        [StringLength(255)]
        public string AdditionalDetails { get; set; }

        [Required]
        public string Gender { get; set; }


        public bool IsEmergency { get; set; }  // Indicates if it's an emergency booking
    }

}

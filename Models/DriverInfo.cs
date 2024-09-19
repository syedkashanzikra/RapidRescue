using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRescue.Models
{
    public class DriverInfo
    {
        [Key]
        public int DriverId { get; set; }  // Primary Key for DriverInfo

     

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }  // Driver's phone number

        [Required]
        [StringLength(100)]
        public string LicenseNumber { get; set; }  // Driver's license number

        [Required(ErrorMessage = "License Expiry Date is required.")]
        public DateTime LicenseExpiryDate { get; set; }  // License expiry date

        [Required]
        [StringLength(200)]
        public string Address { get; set; }  // Driver's address

        [Required]
        [StringLength(100)]
        public string VehicleAssigned { get; set; }  // Vehicle assigned to the driver


        [Required(ErrorMessage = "Date of Hire is required.")]
        public DateTime DateOfHire { get; set; }  // Date when the driver was hired

        [Required]
        public bool IsActive { get; set; }  // Whether the driver is active

        // Foreign key to the Users table
        [Required]
        public int User_id { get; set; }

        [ForeignKey("User_id")]
        public Users Users { get; set; }  // Navigation property to the Users table

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Record creation time

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Record update time

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

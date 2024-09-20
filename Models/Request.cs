using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRescue.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        // Foreign key to the DriverInfo table
        [Required]
        public int DriverId { get; set; }

        [ForeignKey("DriverId")]
        public DriverInfo DriverInfo { get; set; } 

        // Patient's Latitude and Longitude
        [Required]
        public double PatientLatitude { get; set; }

        [Required]
        public double PatientLongitude { get; set; }

        // Time when the request was made
        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;


        public string DriverStatus { get; set; } = "Going to Patient";
    }
}

using System.ComponentModel.DataAnnotations;

namespace RapidRescue.Models
{
    public class AmbulanceRequest
    {
        [Key]
        public string RequestId { get; set; }   // Unique ID for the request
        public string DriverId { get; set; }    // Assigned driver ID
        public double PatientLat { get; set; }  // Patient's latitude
        public double PatientLng { get; set; }  // Patient's longitude
        public DateTime RequestTime { get; set; }  // Timestamp of the request
    }

}

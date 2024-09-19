using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidRescue.Models
{
    public class EMT
    {
        [Key]
        public int EMT_Id { get; set; }

        [Required]
        public int User_id { get; set; }

        [ForeignKey("User_id")]
        public Users Users { get; set; }


        [Required]
        [StringLength(100)]
        public string CertificationNumber { get; set; }  // EMT's certification number

        [Required]
        [DataType(DataType.Date)]
        public DateTime CertificationExpiryDate { get; set; } // Expiry date of certification

        [Required]
        public string ContactNumber { get; set; } // EMT's contact number

        [Required]
        [StringLength(100)]
        public string LicenseNumber { get; set; } // EMT's license number

        [Required]
        public bool IsAvailable { get; set; } 

        [StringLength(200)]
        public string Address { get; set; } // EMT's address

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Record creation time

        [Required]
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Record update time

        
    }
}

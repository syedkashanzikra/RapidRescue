using System;
using System.ComponentModel.DataAnnotations;

namespace RapidRescue.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}

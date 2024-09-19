using System.ComponentModel.DataAnnotations;

namespace RapidRescue.ViewModels
{
    public class CreateEMTViewModel
    {
        // Users Model Properties
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // EMT Model Properties
        [Required]
        [StringLength(100)]
        public string CertificationNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CertificationExpiryDate { get; set; }

        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string LicenseNumber { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        // Other optional validations can go here
    }
}

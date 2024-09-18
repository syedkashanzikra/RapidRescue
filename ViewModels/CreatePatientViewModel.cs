using System.ComponentModel.DataAnnotations;

namespace RapidRescue.ViewModels
{
    public class CreatePatientViewModel
    {

        public int User_id { get; set; }

        public int Patient_Id { get; set; }
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

        // PatientsInfo Model Properties
        [Required]
        [Phone]
        [StringLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string Situation { get; set; }

        [Required]
        [StringLength(255)]
        public string PickupLocation { get; set; }

        [Required]
        [StringLength(255)]
        public string AdditionalDetails { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public bool IsEmergency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Password) && Password.Length < 6)
            {
                yield return new ValidationResult("Password must be at least 6 characters long.", new[] { "Password" });
            }

            if (!string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
            {
                yield return new ValidationResult("Confirm Password is required when changing the password.", new[] { "ConfirmPassword" });
            }

            if (!string.IsNullOrEmpty(ConfirmPassword) && string.IsNullOrEmpty(Password))
            {
                yield return new ValidationResult("Password is required when changing the password.", new[] { "Password" });
            }
        }

    }
}

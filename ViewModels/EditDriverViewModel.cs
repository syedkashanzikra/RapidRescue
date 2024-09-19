using System.ComponentModel.DataAnnotations;

public class EditDriverViewModel
{
    // User-related fields
    public int User_id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }

    // Driver-related fields
    public int DriverInfo_id { get; set; }
    [Required]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "License Number is required.")]
    [RegularExpression(@"^[A-Z0-9]{8,10}$", ErrorMessage = "License number must be alphanumeric and between 8 to 10 characters.")]
    public string LicenseNumber { get; set; }


    [Required]
    public DateTime LicenseExpiryDate { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string VehicleAssigned { get; set; }
    [Required]
    public DateTime DateOfHire { get; set; }

    // Password fields for editing
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

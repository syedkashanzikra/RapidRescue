using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace RapidRescue.Models
{
    public class Users
    {
        [Key]
        public int User_id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } // User's email address

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } 


        [StringLength(100)]
        public string RememberToken { get; set; }


        [Required]
        public int Role_Id { get; set; } 

        [ForeignKey("Role_Id")]
        public Roles Roles { get; set; } 

        [Required]
        public bool IsActive { get; set; }



        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 



    }
}

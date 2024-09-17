using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RapidRescue.Models
{
    public class Roles
    {

        [Key]
        public int Role_Id { get; set; } 

        [Required]
        public string RoleName { get; set; } 

        [AllowNull]
        public bool Status { get; set; }

    }
}

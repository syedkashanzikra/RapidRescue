using System;
using System.ComponentModel.DataAnnotations;

namespace RapidRescue.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string NotificationType { get; set; }

        [Required]
        public string NotificationMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

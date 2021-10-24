namespace TwitterSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [MinLength(2)]
        public string Text { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public DateTime NotificationTime { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}

using GigHub.Models;
using System;

namespace GigHub.Dtos
{
    public class NotificationDto
    {
        public NotificationType NotificationType { get; set; }

        public DateTime DateTime { get; set; }

        public string OriginalVenue { get; set; }

        public DateTime? OriginalDateTime { get; set; }

        public GigDto Gig { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public DateTime DateTime { get; private set; }

        public string OriginalVenue { get; set; }

        public DateTime? OriginalDateTime { get; set; }

        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        public Notification(Gig gig, NotificationType type)
        {
            Gig = gig ?? throw new ArgumentNullException("gig");
            NotificationType = type;
            DateTime = DateTime.Now;
        }

    }
}
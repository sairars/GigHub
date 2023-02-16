using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public DateTime DateTime { get; private set; }

        public string OriginalVenue { get; private set; }

        public DateTime? OriginalDateTime { get; private set; }

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

        public static Notification Update(Gig gig, string venue, DateTime dateTime)
        {
            var notification = new Notification(gig, NotificationType.Updated);

            var isTimeChanged = !DateTime.Equals(gig.DateTime, dateTime);
            if (isTimeChanged)
                notification.OriginalDateTime = gig.DateTime;


            var isVenueChanged = !string.Equals(gig.Venue, venue);
            if (isVenueChanged)
                notification.OriginalVenue = gig.Venue;

            return notification;
        }

        public static Notification Cancel(Gig gig)
        {
            return new Notification(gig, NotificationType.Cancelled);
        }

        public static Notification Create(Gig gig)
        {
            return new Notification(gig, NotificationType.Created);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public bool IsCancelled { get; private set; }
        public string ArtistId { get; set; }
        public ApplicationUser Artist { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
        public ICollection<Attendance> Attendances { get; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCancelled = true;

            var notification = Notification.GenerateGigCancelledNotification(this);
            var attendees = Attendances.Select(a => a.Attendee);

            foreach (var attendee in attendees)
                attendee.Notify(notification);
        }

        public void Update(string venue, DateTime dateTime, byte genreId)
        {
            var notification = Notification.GenerateGigUpdatedNotification(this, venue, dateTime);
            var attendees = Attendances
                .Select(a => a.Attendee);

            foreach (var attendee in attendees)
                attendee.Notify(notification);

            Venue = venue;
            DateTime = dateTime;
            GenreId = genreId;
        }

        public IEnumerable<ApplicationUser> GetAttendees()
        {
            return Attendances.Select(a => a.Attendee);
        }
    }
}
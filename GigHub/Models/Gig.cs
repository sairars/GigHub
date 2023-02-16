﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCancelled { get; private set; }

        [Required]
        public string ArtistId { get; set; }

        public ApplicationUser Artist { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength((255))]
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

            var notification = Notification.Cancel(this);
            var attendees = Attendances.Select(a => a.Attendee);

            foreach (var attendee in attendees)
                attendee.Notify(notification);
        }

        public void Update(string venue, DateTime dateTime, byte genreId)
        {
            var notification = Notification.Update(this, venue, dateTime);
            var attendees = Attendances
                .Select(a => a.Attendee);

            foreach (var attendee in attendees)
                attendee.Notify(notification);

            Venue = venue;
            DateTime = dateTime;
            GenreId = genreId;
        }
    }
}
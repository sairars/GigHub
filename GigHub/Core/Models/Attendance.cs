﻿namespace GigHub.Core.Models
{
    public class Attendance
    {
        public int GigId { get; set; }
        public string AttendeeId { get; set; }
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }
    }
}
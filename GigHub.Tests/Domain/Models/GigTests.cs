using FluentAssertions;
using GigHub.Core.Models;
using NUnit.Framework;
using System.Linq;

namespace GigHub.Tests.Domain.Models
{
    [TestFixture]
    public class GigTests
    {
        [Test]
        public void Cancel_WhenCalled_SetIsCancelledToTrue()
        {
            var gig = new Gig();
            gig.Cancel();
            gig.IsCancelled.Should().BeTrue();
        }

        [Test]
        public void Cancel_WhenCalled_ShouldNotifyAttendees()
        {
            var gig = new Gig();
            gig.Attendances.Add(new Attendance
            {
                Attendee = new ApplicationUser
                {
                    Id = "1"
                }
            });
            gig.Cancel();

            var attendees = gig.GetAttendees().ToList();
            attendees.Count.Should().Be(1);
        }
    }
}
